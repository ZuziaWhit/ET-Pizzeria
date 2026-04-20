# 🍕 Pizza Order Prefab System

## 📌 Overview

This prefab system was created to generate and display randomized pizza orders in a modular and reusable way. The system separates **data**, **logic**, and **UI behavior**, following good software design principles and Unity prefab architecture.

## 🧱 System Architecture

The system is divided into three main components:

### 1. `PizzaOrderData`

* Stores order information
* Contains:

  * Ingredients (List)
  * Bake time
  * Cut type
* Pure data container (no logic)

---

### 2. `PizzaOrderManager`

* Responsible for:

  * Generating random orders
  * Instantiating prefabs
* Ensures:

  * No duplicate ingredients (uses HashSet)
  * Random ingredient count (3–6)
* Acts as the **controller** of the system

---

### 3. `PizzaOrderPrefab`

* Handles UI display
* Displays:

  * Ingredients (one by one using coroutine)
  * Bake time
  * Cut type
* Uses TextMeshPro components

## ⚙️ Setup Instructions

To set up the Pizza Order Prefab system in Unity, first ensure that TextMeshPro is installed by navigating to *Window → TextMeshPro → Import TMP Essentials* and importing the required resources. Next, add the necessary scripts (`PizzaOrderData`, `PizzaOrderManager`, and `PizzaOrderPrefab`) into your project, preferably inside an `Assets/Scripts/` folder for organization. To create the prefab, start by adding a UI Panel in the Hierarchy and renaming it `PizzaOrderPrefab`. Inside this panel, create three TextMeshPro text objects and name them `ToppingsText`, `TimeText`, and `CutText`. Attach the `PizzaOrderPrefab` script to the panel and assign each of the text objects to their corresponding fields in the Inspector. Then, create a new folder called `Prefabs` in the Project window and drag the `PizzaOrderPrefab` object from the Hierarchy into this folder to convert it into a prefab asset. After creating the prefab, remove the instance from the scene so it can be instantiated dynamically at runtime. Next, create an empty GameObject in the Hierarchy, rename it `GameManager`, and attach the `PizzaOrderManager` script to it. In the Inspector, assign the newly created prefab to the `pizzaOrderPrefab` field. If an older script such as `PizzaOrderGenerator` exists in the scene, disable it to avoid conflicts, as only one system should control order generation. Finally, press Play to test the system. A pizza order should appear with 3–6 unique ingredients displayed one at a time, followed by the bake time and cut type. If nothing appears, verify that the prefab is assigned correctly and that the manager script is active. This setup ensures the system functions as a modular, reusable prefab-based architecture.

## Requirements:

Unity 2022.3.42f1 or later