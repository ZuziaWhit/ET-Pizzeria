using UnityEngine;
using System.Collections.Generic;

namespace Assets.src.Audio {

    // Sound Container
    [System.Serializable]
    public class SoundOverride {
        public string soundID;
        [Range(0.5f, 2f)] public float pitchMin = 1f;
        [Range(0.5f, 2f)] public float pitchMax = 1f;    
    }


    public class AudioManager : MonoBehaviour {
        public static AudioManager Instance { get; private set; }

        [Header("Sound Folder")]
        public string soundFolder = "Audio";

        [Header("Per-Sound Pitch Overrides (optional)")]
        public SoundOverride[] pitchOverrides;

        [Header("Audio Source Pool")]
        public int poolSize = 8;

        private Dictionary<string, AudioClip[]> _soundDict;
        private Dictionary<string, (float min, float max)> _pitchOverrideDict;

        private AudioSource[] _pool;
        private int _poolIndex;

        private Camera _mainCamera;


        private void Awake() {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _soundDict         = LoadClipsFromFolder(soundFolder);
            _pitchOverrideDict = BuildPitchOverrideDict();
            poolSize = Mathf.Max(1, poolSize);
            BuildAudioPool();
            _mainCamera        = Camera.main;
        }


        public void PlaySound(string soundID) {
            AudioClip clip = GetClip(soundID);
            if (clip == null) {
                Debug.LogWarning($"[AudioManager] Sound not found: '{soundID}'");
                return;
            }

            AudioSource source = GetPooledSource();
            source.panStereo   = 0f;
            source.clip        = clip;
            source.pitch       = GetPitch(soundID);
            source.Play();
        }

        public void PlaySoundAt(string soundID, Vector3 position) {
            AudioClip clip = GetClip(soundID);
            if (clip == null) {
                Debug.LogWarning($"[AudioManager] Sound not found: '{soundID}'");
                return;
            }

            if (_mainCamera == null) _mainCamera = Camera.main;

            AudioSource source  = GetPooledSource();
            
            if (_mainCamera == null) {
                Debug.LogWarning("[AudioManager] No main camera; falling back to center pan.");
                source.panStereo = 0f;
                source.clip      = clip;
                source.pitch     = GetPitch(soundID);
                source.Play();
                return;
            }

            Vector3 viewportPos = _mainCamera.WorldToViewportPoint(position);
            float pan           = Mathf.Clamp((viewportPos.x * 2f) - 1f, -1f, 1f);

            source.panStereo    = pan;
            source.clip         = clip;
            source.pitch        = GetPitch(soundID);
            source.Play();
        }

        private AudioClip GetClip(string soundID) {
            if (!_soundDict.TryGetValue(soundID, out AudioClip[] clips) || clips.Length == 0)
                return null;

            return clips[Random.Range(0, clips.Length)];
        }

        private float GetPitch(string soundID) {
            if (_pitchOverrideDict.TryGetValue(soundID, out var range))
                return Random.Range(range.min, range.max);

            return 1f;
        }

        // Helpers
        private Dictionary<string, AudioClip[]> LoadClipsFromFolder(string folder) {
            var dict = new Dictionary<string, AudioClip[]>();

            if (string.IsNullOrEmpty(folder)) {
                Debug.LogWarning("[AudioManager] Sound folder path is empty.");
                return dict;
            }

            AudioClip[] clips = Resources.LoadAll<AudioClip>(folder);

            if (clips == null || clips.Length == 0) {
                Debug.LogWarning($"[AudioManager] No AudioClips found in Resources/{folder}");
                return dict;
            }

            var groups = new Dictionary<string, List<AudioClip>>();

            foreach (AudioClip clip in clips) {
                string key = StripVariantSuffix(clip.name);
                if (!groups.ContainsKey(key)) groups[key] = new List<AudioClip>();
                groups[key].Add(clip);
            }

            foreach (var kvp in groups)
                dict[kvp.Key] = kvp.Value.ToArray();

            Debug.Log($"[AudioManager] Loaded {clips.Length} clip(s) as {dict.Count} soundID(s) from Resources/{folder}");
            return dict;
        }

        private string StripVariantSuffix(string clipName) {
            int underscore = clipName.LastIndexOf('_');
            if (underscore < 0) return clipName;

            string suffix = clipName.Substring(underscore + 1);
            if (int.TryParse(suffix, out _))
                return clipName.Substring(0, underscore);

            return clipName;
        }

        private Dictionary<string, (float min, float max)> BuildPitchOverrideDict() {
            var dict = new Dictionary<string, (float, float)>();
            if (pitchOverrides == null) return dict;

            foreach (var ov in pitchOverrides) {
                if (string.IsNullOrEmpty(ov.soundID)) continue;
                dict[ov.soundID] = (ov.pitchMin, ov.pitchMax);
            }
            return dict;
        }

        private void BuildAudioPool() {
            _pool = new AudioSource[poolSize];
            for (int i = 0; i < poolSize; i++) {
                GameObject go = new GameObject($"AudioSource_Pool_{i}");
                go.transform.SetParent(transform);
                AudioSource src  = go.AddComponent<AudioSource>();
                src.playOnAwake  = false;
                _pool[i]         = src;
            }
        }

        private AudioSource GetPooledSource() {
            for (int i = 0; i < poolSize; i++) {
                int idx = (_poolIndex + i) % poolSize;
                if (!_pool[idx].isPlaying) {
                    _poolIndex = (idx + 1) % poolSize;
                    return _pool[idx];
                }
            }

            AudioSource stolen = _pool[_poolIndex];
            stolen.Stop();
            _poolIndex = (_poolIndex + 1) % poolSize;
            return stolen;
        }
    }
}
