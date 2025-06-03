# HIZLI TEST SETUP - Bhop Movement

## 1. Sahne Kurulumu (1 dakika)

### Ground (Zemin) Olu�tur:
1. Hierarchy ? 3D Object ? Cube
2. Scale: (20, 1, 20)
3. Position: (0, 0, 0)
4. Rename: "Ground"

### Player Olu�tur:
1. Hierarchy ? 3D Object ? Capsule
2. Position: (0, 2, 0)
3. Rename: "Player"
4. **BhopMovement.cs** scriptini s�r�kle
5. CharacterController otomatik eklenecek

### Camera Setup:
1. Player alt�nda yeni bo� GameObject ? "CameraHolder"
2. CameraHolder Position: (0, 0.8, 0)
3. Main Camera'y� CameraHolder'�n child'� yap
4. Main Camera Position: (0, 0, 0)
5. Player'daki BhopMovement script'te Camera referans�n� ata

## 2. HIZLI TEST:

### Kontroller:
- **WASD**: Hareket
- **Space**: Z�plama (tek seferlik)
- **Mouse**: Bak�� + strafe boost
- **F5**: Debug console log

### Jump Modu:
- **Normal**: Space ile tek z�plama (Enable Bhop Queue = false)
- **Bhop**: Space bas�l� tutarak arka arkaya z�plama (Enable Bhop Queue = true)

### Test Ad�mlar�:
1. Play'e bas
2. Space ile z�pla - tek z�plama yapmal�
3. WASD ile hareket et
4. F5 ile debug bilgilerini kontrol et
5. Bhop istiyorsan Inspector'da "Enable Bhop Queue" aktifle�tir

## 3. Sorun Giderme:

### Z�playam�yorsan:
1. **Console'u kontrol et** - "SPACE PRESSED!" yaz�yor mu?
2. **Ground check** - Scene'de ye�il �izgi g�r�yor musun?
3. **Player Scale** - �ok b�y�k/k���k olabilir
4. **Ground Layer** - Layer ayarlar�n� kontrol et

### Debug Bilgileri:
- F5: Console'da detayl� debug
- Scene'de Gizmos: Velocity (mavi), Movement (k�rm�z�)
- Ye�il ray = grounded, k�rm�z� ray = havada

## 4. ?? BHOP + SLOPE TEST (G�ncellenmi�):

### E�imli Rampa Olu�tur:
1. Hierarchy ? 3D Object ? Cube
2. Scale: (8, 0.5, 15) - Uzun rampa
3. Position: (0, 3, 12)
4. Rotation: (20, 0, 0) - 20� e�im
5. Rename: "SlopeRamp"

### ?? Mouse Movement + Speed Boost Test:
1. **Play'e bas**
2. **Fareyi sa�a-sola hareket ettir** (hem yerdeyken hem havadayken)
3. **F5'e bas** - "MOUSE BOOST" loglar�n� g�r
4. **Speed art���n� g�zlemle** (Debug UI'da)
5. **Test**: Havadayken 2x daha fazla boost almal�s�n

### ?? Slope Sliding Test:
1. **Rampaya ��k**
2. **F5'e bas ve bas�l� tut** 
3. **Console'da kontrol et:**
   - "SLOPE DEBUG" - A�� 20� civar� g�r�nmeli
   - "SLOPE SLIDING ACTIVE!" - Kayma �al���yor
   - "Velocity BEFORE/AFTER" - H�z art��� var
4. **Rampadan a�a�� in** - H�zlanmal�s�n

### ?? Kombinasyon Test (En Etkili):
1. **Rampaya A/D + mouse ile yakla�**
2. **Space ile z�pla**
3. **Havadayken fareyi h�zla sa�a-sola**
4. **A/D ile strafe yap (W basma!)**
5. **Rampaya de�ince otomatik h�zlan**

## 5. ?? Optimum Bhop Ayarlar�:

Inspector'da bu de�erleri dene:
```
Mouse Speed Boost Multiplier: 0.05 (art�r�ld�!)
Max Speed Boost: 25
Slope Force: 35 (art�r�ld�!)
Min Slope Angle: 5� (azalt�ld�!)
Mouse Sensitivity: 2.5
```

## 6. ?? Ba�ar� Kriterleri:

? **Mouse Hareket = H�z Boost** (hem yer hem hava)
? **Yoku� a�a�� = Otomatik h�zlanma**  
? **Strafe + Mouse = Maksimum h�z**
? **Debug UI'da t�m bilgiler g�r�n�r**

### ? Sorun Giderme:
- **Mouse boost yok**: Multiplier'� 0.1'e ��kar
- **Slope sliding yok**: Force'u 50'ye ��kar, Min Angle'� 3'e d���r
- **Az h�zlanma**: Max Speed Boost'u 35'e ��kar

**?? PRO T�P**: En iyi bhop i�in W tu�una basma, sadece A/D + mouse hareket kullan!
