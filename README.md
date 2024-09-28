# Richter-Pinata

 A simple mini game where the player need to press on the pinata to win prizes!
 There are 3 levels of the same game each different from the other (try to think what the levels indicates)
 
 [APK](https://drive.google.com/drive/folders/1eax7FJHQfvGJ7bz9exJQm4X0jOgGeaGQ?usp=sharing)

 How to play:
 Press on the Play Pinata button to start
 Press many times on the pinaat until it explodes.
 Repeat.
 You can select the level you want by press on each of the 3 buttons at the bottom.

 [Gameplay Video](https://drive.google.com/file/d/1rTME_LAqMXc5wEdVHmbbJb5HJaAJg86u/view?usp=drive_link)

 ![image](https://github.com/user-attachments/assets/7119e065-7084-4c13-9c71-99802c064cd4)


 I used mostly hte assets that were given from the task.
 I used a font from https://www.dafont.com/loli-pop.font?text=Lets+POP+a+Pinata
 Created by myself:
 shaders (pinata and background)
 lights texture
 haptic feedback

 In the project I used:
 DOTWEEN
 TMP
 URP Rendering
 and I used async Task (not Unitask - for no particular reason just the scope)

 I made the game to be able to add more and more features based on requirement (modular)
 Each element inherite from PinataListener and this can add many functionalites or features in the experience on hit and on explosion.
 Using the settings I can modify the levels add more levels and effect based on the requirements.

Haptic feedback is created thorugh Java and Android studio (.aar) file, also There is a test window you can try and see the vibration sequence
![image](https://github.com/user-attachments/assets/bde033bb-abaa-4ac6-bace-455c27e53a56)


