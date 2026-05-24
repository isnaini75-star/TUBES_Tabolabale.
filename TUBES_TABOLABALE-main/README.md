### Cara Build Game Engine

1. Algoritma Greedy pada Bot Incess
   Bot Incess menggunakan strategi Greedy Aggressive & Bullet Damage Maksimal.
Strategi greedy adalah metode pengambilan keputusan yang selalu memilih aksi terbaik pada saat 
itu juga tanpa mempertimbangkan kondisi jangka panjang. Keputusan yang diambil pada algoritma ini adalah:
   - Lock Radar dan Gun ke Musuh
      Saat musuh terdeteksi melalui radar, bot langsung memfokuskan radar dan turret 
      ke posisi musuh agar tracking lebih akurat.
   - Menembak dengan Damage Maksimum
      Jika arah gun sudah hampir tepat ke musuh (GunTurnRemaining < 8), 
      bot langsung menembak menggunakan power maksimum (Fire(3)).
   - Mengejar Musuh
      Jika musuh berada jauh (distance > 150), bot akan maju mendekati lawan agar dapat menyerang lebih agresif.
   - Gerakan ZigZag
      Bot bergerak zigzag secara terus-menerus untuk mempersulit lawan mengenai target.
   - Dodge Saat Kena Peluru
      Ketika terkena peluru, bot langsung mengubah arah gerak secara cepat menggunakan pola zigzag untuk menghindari serangan berikutnya.
   - Agresif Saat Menabrak Musuh
      Ketika bertabrakan dengan lawan, bot langsung menembak dengan power maksimum dan terus maju menekan musuh.

2. Reqruitment program dan Instalasi
   - .NetSDK vers.10
   - Robocode Tank Royale
   - Java Development Kit

   File yang harus ada
   - C# Source File
   - JSON Source File
   - SH Source File
   - c# Project Source file
   - Windows Command
   
3. Command Atau Cara Build Program
   - Buka terminal Command
      Setelah membuka cmd, lalu ketik cd kemudian salin folder project dari bot incess. 
   - Build Program
      Setelah itu ketik dotnet build, apabila sudah mendapatkan hasil 'build successed' maka bot berhasil dijalankan.
      Sehingga Bot akan otomatis terkoneksi ke Robocode Tank Royale jika server arena sudah dijalankan.

4. Authors
   Nama: Isnaini Febriana (124140075), Putu Laura Claudia Ardani (124140021), Cania Febriyanti (124140153)
   Bahasa Pemrograman: C#
   Framework: Robocode Tank Royale
   Strategi Bot: Greedy Aggressive & Bullet Damage Maksimal
   Bot Name: Incess

