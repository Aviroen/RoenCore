Many thanks to Button for handholding me the entire time and realizing I'm dumb (TM)

Content Patcher capability to add in a Custom Field to Data/Characters

**REMOVING ADOPTION/PREGNANCY RANDOMIZATION**

The syntax for that is:
```
{
  "Action": "EditData",
  "Target": "Data/Characters",
  "Fields": {
    "Abigail": {
      "CustomFields": {
        "Aviroen.GSQBaby": "true"
      }
    }
  }
}
```

This example is targeting Abigail, and editing her CustomField to be my UniqueID, since it is String -> String, it will try and parse the "text" to be a bool.

If this is left out, you get the standard RNG for adoption/pregnancy question.

/////////////////////////// (I hate formatting GitHub sorry this is gonna be ugly)

Newest chaotic addition: "BuildingType": "Aviroen.AtraAntiCrow" in Data/Buildings will let you have a custom building that scares off all crows.

```
{
  "Action": "EditData",
  "Target": "Data/Buildings",
  "Fields": {
    "Deluxe Coop": {
      "CustomFields": {
        "Aviroen.AtraAntiCrow": "true"
      }
    }
  }
}
```
