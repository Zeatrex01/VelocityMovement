# Bhop Movement Debug UI Setup Guide

Bu dosya, Unity'de Bhop Movement sistemi için debug UI'larýnýn nasýl kurulacaðýný açýklar.

## 1. Basic Setup

### Player GameObject Setup:
1. Player GameObject'e `BhopMovement.cs` script'ini ekleyin
2. CharacterController component'i ekleyin (otomatik eklenir)
3. Camera'yý player'ýn child'ý yapýn

### Camera Setup:
- Position: (0, 1.8, 0) - göz hizasý
- Player Script'te Camera referansýný atayýn

## 2. Debug UI Setup

### Canvas Oluþturma:
1. Hierarchy'de sað týk ? UI ? Canvas
2. Canvas'ý "Debug Canvas" olarak adlandýrýn
3. Canvas Scaler ekleyin:
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1920x1080

### Debug Info Panel:
1. Canvas altýnda boþ GameObject oluþturun ? "Debug Panel"
2. RectTransform ayarlarý:
   - Anchor: Sol üst (0,1)
   - Position: (10, -10)
   - Size: (300, 150)

### Text Elementleri Oluþturma:
Debug Panel altýnda TextMeshPro - Text (UI) elementleri oluþturun:

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
- Text: "Ground: Flat (0.0°)"
- Font Size: 14
- Color: Green

## 3. Performance Monitor Panel

### Performance Panel:
1. Canvas altýnda yeni boþ GameObject ? "Performance Panel"
2. RectTransform ayarlarý:
   - Anchor: Sað üst (1,1)
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

**NOT:** Tüm text elementleri için UI ? TextMeshPro - Text (UI) kullanýn, eski Text component'ini deðil.

## 4. Script Baðlantýlarý

### BhopDebugUI Component:
1. Debug Panel GameObject'e `BhopDebugUI.cs` script'ini ekleyin
2. Inspector'da UI References'larý ayarlayýn:
   - Speed Text ? "Speed Text" objesini sürükleyin
   - Boost Text ? "Mouse Speed Text" objesini sürükleyin
   - Grounded Text ? "Grounded Text" objesini sürükleyin
   - Top Speed Text ? "Top Speed Text" objesini sürükleyin
   - Velocity Text ? "Velocity Text" objesini sürükleyin
   - Input Text ? "Input Text" objesini sürükleyin
   - Slope Text ? "Slope Text" objesini sürükleyin (yeni eklendi)

### BhopPerformanceMonitor Component:
1. Performance Panel GameObject'e `BhopPerformanceMonitor.cs` script'ini ekleyin
2. Inspector'da UI References'larý ayarlayýn:
   - FPS Text ? "FPS Text" objesini sürükleyin
   - Frame Time Text ? "Frame Time Text" objesini sürükleyin
   - Memory Text ? "Memory Text" objesini sürükleyin
   - Jump Count Text ? "Jump Count Text" objesini sürükleyin
   - Air Time Text ? "Air Time Text" objesini sürükleyin

## 5. Kontroller

### Klavye Kýsayollarý:
- **F3**: Debug UI'yi aç/kapat
- **F4**: Performance Monitor'u aç/kapat
- **R**: Performance istatistiklerini sýfýrla

### Movement Kontrolleri:
- **WASD**: Hareket
- **Mouse**: Kamera/bakýþ yönü
- **Space**: Zýplama (basýlý tutarak bhop)
- **Shift**: Koþma (opsiyonel)

## 6. Debug Bilgileri

### Debug UI Göstergeleri:
- **Speed**: Anlýk hareket hýzý
- **Mouse Speed**: Mouse hareket hýzý (strafe jumping için)
- **Grounded**: Yerde olup olmama durumu
- **Top Speed**: Ulaþýlan maksimum hýz
- **Velocity**: 3D velocity vektörü
- **Input**: Hareket input deðerleri
- **Ground/Slope**: Zemin durumu ve eðim bilgisi (yeni eklendi)

### Performance Göstergeleri:
- **FPS**: Saniyedeki frame sayýsý
- **Frame Time**: Ortalama frame süresi
- **Memory**: Kullanýlan bellek miktarý
- **Jumps**: Toplam zýplama sayýsý
- **Avg Air Time**: Ortalama havada kalma süresi

## 7. Slope Sliding Sistemi (Yeni Eklendi)

### Slope Sliding Özellikleri:
- **Slope Force**: Eðimlerde kayma kuvveti (varsayýlan: 8)
- **Min Slope Angle**: Minimum kayma açýsý (varsayýlan: 15°)
- **Max Slope Angle**: Maksimum kayma açýsý (varsayýlan: 45°)
- **Slope Ray Length**: Zemin tespiti için ray uzunluðu (varsayýlan: 1.5)
- **Enable Slope Sliding**: Eðim kayma sistemini aç/kapat

### Slope Sliding Nasýl Çalýþýr:
1. **Flat Ground (0-5°)**: Normal hareket, kayma yok
2. **Sliding Slopes (15-45°)**: Otomatik kayma aktif
3. **Steep Slopes (45°+)**: Kayma yok (çok dik)

### Slope Debug Göstergeleri:
- **"Ground: Flat"**: Düz zemin
- **"Ground: SLIDING"**: Aktif kayma durumu
- **"Ground: Slope"**: Eðim var ama kayma yok
- **"Ground: Not Grounded"**: Havada

## 7. Optimizasyon Notlarý

- Update Rate'leri ayarlayarak performansý optimize edebilirsiniz
- Mobil platformlarda daha düþük update rate'leri kullanýn
- UI elementlerini ihtiyaç olmadýðýnda kapatýn
- Performance Monitor'u sadece geliþtirme sýrasýnda kullanýn

## 8. Sorun Giderme

### Yaygýn Sorunlar:
1. **UI görünmüyor**: Canvas'ýn Render Mode'unun Screen Space - Overlay olduðundan emin olun
2. **Script referanslarý bozuk**: Inspector'da UI referanslarýný yeniden atayýn
3. **Performance düþük**: Update Rate'leri artýrýn (0.1 ? 0.2)
4. **Mobile'da sorun**: Touch input ayarlarýný kontrol edin

### Debug Komutlarý:
```csharp
// Console'da debug bilgilerini görmek için:
// F4 tuþuna basýn veya kod içinde:
Debug.Log("Movement Debug Info");
```

Bu setup'ý tamamladýktan sonra, Unity'de test edebilir ve gerektiðinde ayarlarý deðiþtirebilirsiniz.
