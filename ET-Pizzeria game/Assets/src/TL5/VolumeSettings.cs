public class VolumeSettings
{
    private class VolumeData
    {
        public float volume;

        public VolumeData(float volume)
        {
            this.volume = volume;
        }
    }

    private VolumeData data;

    public VolumeSettings(float volume)
    {
        data = new VolumeData(volume);
    }

    public float GetVolume()
    {
        return data.volume;
    }
}