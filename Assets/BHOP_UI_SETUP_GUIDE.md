# Bhop Movement Debug UI Setup Guide

Bu dosya, Unity'de Bhop Movement sistemi i�in debug UI'lar�n�n nas�l kurulaca��n� a��klar.

## 1. Basic Setup

### Player GameObject Setup:
1. Player GameObject'e `BhopMovement.cs` script'ini ekleyin
2. CharacterController component'i ekleyin (otomatik eklenir)
3. Camera'y� player'�n child'� yap�n

### Camera Setup:
- Position: (0, 1.8, 0) - g�z hizas�
- Player Script'te Camera referans�n� atay�n

## 2. Debug UI Setup

### Canvas Olu�turma:
1. Hierarchy'de sa� t�k ? UI ? Canvas
2. Canvas'� "Debug Canvas" olarak adland�r�n
3. Canvas Scaler ekleyin:
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920x1080

### Debug Info Panel:
1. Canvas alt�nda bo� GameObject olu�turun ? "Debug Panel"
2. RectTransform ayarlar�:
   - Anchor: Sol �st (0,1)
   - Position: (10, -10)
   - Size: (300, 150)

### Text Elementleri Olu�turma:
Debug Panel alt�nda TextMeshPro - Text (UI) elementleri olu�turun:

#### Speed Text:
- Name: "Speed Text"
- Position: (10, -20)
- Text: "Speed: 0.0 u/s"
- Font Size: 14
- Color: White

#### Mouse Speed Text:
- Name: "Mouse Speed Text"
- Position: (10, -40)
- Text: "Mouse Speed: 0.00"
- Font Size: 14
- Color: Cyan

#### Grounded Text:
- Name: "Grounded Text"
- Position: (10, -60)
- Text: "Grounded: Yes"
- Font Size: 14
- Color: Green

#### Top Speed Text:
- Name: "Top Speed Text"
- Position: (10, -80)
- Text: "Top Speed: 0.0"
- Font Size: 14
- Color: Magenta

#### Velocity Text:
- Name: "Velocity Text"
- Position: (10, -100)
- Text: "Velocity: (0, 0, 0)"
- Font Size: 14
- Color: White

#### Input Text:
- Name: "Input Text"
- Position: (10, -120)
- Text: "Input: (0, 0)"
- Font Size: 14
- Color: Gray

#### Slope Text (Yeni Eklendi):
- Name: "Slope Text"
- Position: (10, -140)
- Text: "Ground: Flat (0.0�)"
- Font Size: 14
- Color: Green

## 3. Performance Monitor Panel

### Performance Panel:
1. Canvas alt�nda yeni bo� GameObject ? "Performance Panel"
2. RectTransform ayarlar�:
   - Anchor: Sa� �st (1,1)
   - Position: (-10, -10)
   - Size: (250, 120)

### Performance Text Elementleri:

#### FPS Text:
- Name: "FPS Text"
- Position: (-240, -20)
- Text: "FPS: 60"
- Font Size: 14
- Color: Green

#### Frame Time Text:
- Name: "Frame Time Text"
- Position: (-240, -40)
- Text: "Frame Time: 16.7ms"
- Font Size: 14
- Color: White

#### Memory Text:
- Name: "Memory Text"
- Position: (-240, -60)
- Text: "Memory: 100MB"
- Font Size: 14
- Color: Cyan

#### Jump Count Text:
- Name: "Jump Count Text"
- Position: (-240, -80)
- Text: "Jumps: 0"
- Font Size: 14
- Color: Magenta

#### Air Time Text:
- Name: "Air Time Text"
- Position: (-240, -100)
- Text: "Avg Air Time: 0.0s"
- Font Size: 14
- Color: Yellow

**NOT:** T�m text elementleri i�in UI ? TextMeshPro - Text (UI) kullan�n, eski Text component'ini de�il.

## 4. Script Ba�lant�lar�

### BhopDebugUI Component:
1. Debug Panel GameObject'e `BhopDebugUI.cs` script'ini ekleyin
2. Inspector'da UI References'lar� ayarlay�n:
   - Speed Text ? "Speed Text" objesini s�r�kleyin
   - Boost Text ? "Mouse Speed Text" objesini s�r�kleyin
   - Grounded Text ? "Grounded Text" objesini s�r�kleyin
   - Top Speed Text ? "Top Speed Text" objesini s�r�kleyin
   - Velocity Text ? "Velocity Text" objesini s�r�kleyin
   - Input Text ? "Input Text" objesini s�r�kleyin
   - Slope Text ? "Slope Text" objesini s�r�kleyin (yeni eklendi)

### BhopPerformanceMonitor Component:
1. Performance Panel GameObject'e `BhopPerformanceMonitor.cs` script'ini ekleyin
2. Inspector'da UI References'lar� ayarlay�n:
   - FPS Text ? "FPS Text" objesini s�r�kleyin
   - Frame Time Text ? "Frame Time Text" objesini s�r�kleyin
   - Memory Text ? "Memory Text" objesini s�r�kleyin
   - Jump Count Text ? "Jump Count Text" objesini s�r�kleyin
   - Air Time Text ? "Air Time Text" objesini s�r�kleyin

## 5. Kontroller

### Klavye K�sayollar�:
- **F3**: Debug UI'yi a�/kapat
- **F4**: Performance Monitor'u a�/kapat
- **R**: Performance istatistiklerini s�f�rla

### Movement Kontrolleri:
- **WASD**: Hareket
- **Mouse**: Kamera/bak�� y�n�
- **Space**: Z�plama (bas�l� tutarak bhop)
- **Shift**: Ko�ma (opsiyonel)

## 6. Debug Bilgileri

### Debug UI G�stergeleri:
- **Speed**: Anl�k hareket h�z�
- **Mouse Speed**: Mouse hareket h�z� (strafe jumping i�in)
- **Grounded**: Yerde olup olmama durumu
- **Top Speed**: Ula��lan maksimum h�z
- **Velocity**: 3D velocity vekt�r�
- **Input**: Hareket input de�erleri
- **Ground/Slope**: Zemin durumu ve e�im bilgisi (yeni eklendi)

### Performance G�stergeleri:
- **FPS**: Saniyedeki frame say�s�
- **Frame Time**: Ortalama frame s�resi
- **Memory**: Kullan�lan bellek miktar�
- **Jumps**: Toplam z�plama say�s�
- **Avg Air Time**: Ortalama havada kalma s�resi

## 7. Slope Sliding Sistemi (Yeni Eklendi)

### Slope Sliding �zellikleri:
- **Slope Force**: E�imlerde kayma kuvveti (varsay�lan: 8)
- **Min Slope Angle**: Minimum kayma a��s� (varsay�lan: 15�)
- **Max Slope Angle**: Maksimum kayma a��s� (varsay�lan: 45�)
- **Slope Ray Length**: Zemin tespiti i�in ray uzunlu�u (varsay�lan: 1.5)
- **Enable Slope Sliding**: E�im kayma sistemini a�/kapat

### Slope Sliding Nas�l �al���r:
1. **Flat Ground (0-5�)**: Normal hareket, kayma yok
2. **Sliding Slopes (15-45�)**: Otomatik kayma aktif
3. **Steep Slopes (45�+)**: Kayma yok (�ok dik)

### Slope Debug G�stergeleri:
- **"Ground: Flat"**: D�z zemin
- **"Ground: SLIDING"**: Aktif kayma durumu
- **"Ground: Slope"**: E�im var ama kayma yok
- **"Ground: Not Grounded"**: Havada

## 7. Optimizasyon Notlar�

- Update Rate'leri ayarlayarak performans� optimize edebilirsiniz
- Mobil platformlarda daha d���k update rate'leri kullan�n
- UI elementlerini ihtiya� olmad���nda kapat�n
- Performance Monitor'u sadece geli�tirme s�ras�nda kullan�n

## 8. Sorun Giderme

### Yayg�n Sorunlar:
1. **UI g�r�nm�yor**: Canvas'�n Render Mode'unun Screen Space - Overlay oldu�undan emin olun
2. **Script referanslar� bozuk**: Inspector'da UI referanslar�n� yeniden atay�n
3. **Performance d���k**: Update Rate'leri art�r�n (0.1 ? 0.2)
4. **Mobile'da sorun**: Touch input ayarlar�n� kontrol edin

### Debug Komutlar�:
```csharp
// Console'da debug bilgilerini g�rmek i�in:
// F4 tu�una bas�n veya kod i�inde:
Debug.Log("Movement Debug Info");
```

Bu setup'� tamamlad�ktan sonra, Unity'de test edebilir ve gerekti�inde ayarlar� de�i�tirebilirsiniz.
