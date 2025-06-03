# HIZLI TEST SETUP - Bhop Movement

## 1. Sahne Kurulumu (1 dakika)

### Ground (Zemin) Oluþtur:
1. Hierarchy ? 3D Object ? Cube
2. Scale: (20, 1, 20)
3. Position: (0, 0, 0)
4. Rename: "Ground"

### Player Oluþtur:
1. Hierarchy ? 3D Object ? Capsule
2. Position: (0, 2, 0)
3. Rename: "Player"
4. **BhopMovement.cs** scriptini sürükle
5. CharacterController otomatik eklenecek

### Camera Setup:
1. Player altýnda yeni boþ GameObject ? "CameraHolder"
2. CameraHolder Position: (0, 0.8, 0)
3. Main Camera'yý CameraHolder'ýn child'ý yap
4. Main Camera Position: (0, 0, 0)
5. Player'daki BhopMovement script'te Camera referansýný ata

## 2. HIZLI TEST:

### Kontroller:
- **WASD**: Hareket
- **Space**: Zýplama (tek seferlik)
- **Mouse**: Bakýþ + strafe boost
- **F5**: Debug console log

### Jump Modu:
- **Normal**: Space ile tek zýplama (Enable Bhop Queue = false)
- **Bhop**: Space basýlý tutarak arka arkaya zýplama (Enable Bhop Queue = true)

### Test Adýmlarý:
1. Play'e bas
2. Space ile zýpla - tek zýplama yapmalý
3. WASD ile hareket et
4. F5 ile debug bilgilerini kontrol et
5. Bhop istiyorsan Inspector'da "Enable Bhop Queue" aktifleþtir

## 3. Sorun Giderme:

### Zýplayamýyorsan:
1. **Console'u kontrol et** - "SPACE PRESSED!" yazýyor mu?
2. **Ground check** - Scene'de yeþil çizgi görüyor musun?
3. **Player Scale** - Çok büyük/küçük olabilir
4. **Ground Layer** - Layer ayarlarýný kontrol et

### Debug Bilgileri:
- F5: Console'da detaylý debug
- Scene'de Gizmos: Velocity (mavi), Movement (kýrmýzý)
- Yeþil ray = grounded, kýrmýzý ray = havada

## 4. ?? BHOP + SLOPE TEST (Güncellenmiþ):

### Eðimli Rampa Oluþtur:
1. Hierarchy ? 3D Object ? Cube
2. Scale: (8, 0.5, 15) - Uzun rampa
3. Position: (0, 3, 12)
4. Rotation: (20, 0, 0) - 20° eðim
5. Rename: "SlopeRamp"

### ?? Mouse Movement + Speed Boost Test:
1. **Play'e bas**
2. **Fareyi saða-sola hareket ettir** (hem yerdeyken hem havadayken)
3. **F5'e bas** - "MOUSE BOOST" loglarýný gör
4. **Speed artýþýný gözlemle** (Debug UI'da)
5. **Test**: Havadayken 2x daha fazla boost almalýsýn

### ?? Slope Sliding Test:
1. **Rampaya çýk**
2. **F5'e bas ve basýlý tut** 
3. **Console'da kontrol et:**
   - "SLOPE DEBUG" - Açý 20° civarý görünmeli
   - "SLOPE SLIDING ACTIVE!" - Kayma çalýþýyor
   - "Velocity BEFORE/AFTER" - Hýz artýþý var
4. **Rampadan aþaðý in** - Hýzlanmalýsýn

### ?? Kombinasyon Test (En Etkili):
1. **Rampaya A/D + mouse ile yaklaþ**
2. **Space ile zýpla**
3. **Havadayken fareyi hýzla saða-sola**
4. **A/D ile strafe yap (W basma!)**
5. **Rampaya deðince otomatik hýzlan**

## 5. ?? Optimum Bhop Ayarlarý:

Inspector'da bu deðerleri dene:
```
Mouse Speed Boost Multiplier: 0.05 (artýrýldý!)
Max Speed Boost: 25
Slope Force: 35 (artýrýldý!)
Min Slope Angle: 5° (azaltýldý!)
Mouse Sensitivity: 2.5
```

## 6. ?? Baþarý Kriterleri:

? **Mouse Hareket = Hýz Boost** (hem yer hem hava)
? **Yokuþ aþaðý = Otomatik hýzlanma**  
? **Strafe + Mouse = Maksimum hýz**
? **Debug UI'da tüm bilgiler görünür**

### ? Sorun Giderme:
- **Mouse boost yok**: Multiplier'ý 0.1'e çýkar
- **Slope sliding yok**: Force'u 50'ye çýkar, Min Angle'ý 3'e düþür
- **Az hýzlanma**: Max Speed Boost'u 35'e çýkar

**?? PRO TÝP**: En iyi bhop için W tuþuna basma, sadece A/D + mouse hareket kullan!
