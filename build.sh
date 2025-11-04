#!/bin/bash
dotnet build
# mkdir '/mnt/sofia/SteamLibrary/steamapps/compatdata/544550/pfx/drive_c/users/steamuser/Documents/My Games/Stationeers/mods/MediumDishCorrection/'
echo Copying dll to Stationeers directory
cp bin/Debug/net48/MediumDishCorrection.dll '/mnt/sofia/SteamLibrary/steamapps/compatdata/544550/pfx/drive_c/users/steamuser/Documents/My Games/Stationeers/mods/MediumDishCorrection/'
echo Copying pdb to Stationeers directory
cp bin/Debug/net48/MediumDishCorrection.pdb '/mnt/sofia/SteamLibrary/steamapps/compatdata/544550/pfx/drive_c/users/steamuser/Documents/My Games/Stationeers/mods/MediumDishCorrection/'
echo Copying GameData to Stationeers directory
cp -r GameData '/mnt/sofia/SteamLibrary/steamapps/compatdata/544550/pfx/drive_c/users/steamuser/Documents/My Games/Stationeers/mods/MediumDishCorrection/'
echo Copying About to Stationeers directory
cp -r About '/mnt/sofia/SteamLibrary/steamapps/compatdata/544550/pfx/drive_c/users/steamuser/Documents/My Games/Stationeers/mods/MediumDishCorrection/'
