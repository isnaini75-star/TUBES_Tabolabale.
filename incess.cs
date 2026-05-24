// Library dasar C#
using System; 

// Library untuk warna bot
using System.Drawing;

// Library utama Robocode Tank Royale
using Robocode.TankRoyale.BotApi;

// Library event seperti scan musuh, kena peluru, dll
using Robocode.TankRoyale.BotApi.Events;

// Bot yang bernama Incess Menggunakan Startegy Greedy yaitu Aggressive & Bullet Damage Maksimal
// ------------------------------------------------------------------
// Bot ini menggunakan strategi greedy agresif.
// Bot selalu memilih aksi terbaik saat ini tanpa mikirin jangka panjang
// yaitu:
// 1. Mengunci musuh
// 2. Mendekati musuh
// 3. Menembak dengan damage maksimum
// 4. Bergerak zigzag agar sulit ditembak

// Heuristik yang digunakan pada Strategy Greedy:
// - Jika musuh terlihat bot akan langsung lock radar & gun
// - Jika gun hampir tepat bot akan langsung tembak power besar
// - Jika musuh jauh bot akan mengejar lalu mendekati musuh
// - Jika kena peluru bot akan dodge zigzag

// Jenis greedy:
// Greedy Aggressive & Bullet Damage Maksimal

public class Incess : Bot
{
    // Variabel yang digunakan untuk mengecek apakah bot sedang maju atau mundur
    // Yang nantinya akan digunakan pada method ReverseDirection()
    private bool movingForward = true;

    // =============
    // METHOD MAIN
    // =============
    static void Main()
    {
        // Membuat object bot dan kemudian dijalankan
        new Incess().Start();
    }

    // =================
    // CONSTRUCTOR BOT
    // =================
    // Yang digunakan untuk mengambil informasi bot dari file JSON
    Incess() : base(BotInfo.FromFile("Incess.json")) { }

    // ============
    // METHOD RUN
    // ============
    // Main Method yang nantinya terus berjalan selama bot hidup
    public override void Run()
    {
        // PENGATURAN WARNA BOT
        BodyColor   = Color.FromArgb(0xFF, 0x69, 0xB4); // hot pink 
        TurretColor = Color.FromArgb(0xFF, 0xFF, 0x00); // kuning
        RadarColor  = Color.FromArgb(0xFF, 0x14, 0x93); // dark pink 
        BulletColor = Color.FromArgb(0x90, 0xEE, 0x90); // light green
        ScanColor   = Color.FromArgb(0xFF, 0xC0, 0xCB); // light pink

        // =========================
        // PENGATURAN GUN & RADAR
        // =========================
        // Gun tidak ikut berputar saat body berputar supaya aim tetap stabil
        AdjustGunForBodyTurn = true;

        // Radar tidak ikut berputar saat gun berputar supaya radar fokus scan musuh
        AdjustRadarForGunTurn = true;

        // =========================
        // LOOP UTAMA BOT INCESS
        // =========================
        while (IsRunning)
        {
            // Radar terus berputar 360 derajat
            SetTurnRadarRight(360);

            // ==================
            // GERAKAN ZIGZAG
            // ==================

            // ZIG KANAN
            // Bot maju 180 pixel
            SetForward(180);
            // Sambil belok kanan 40 derajat
            SetTurnRight(40);

            // Menjalankan perintah movement
            Go();

            // ZIG KIRI
            // Bot maju lagi
            SetForward(180);
            // Belok kiri 80 derajat
            SetTurnLeft(80);

            // Jalankan movement
            Go();

            //ZIG KANAN
            // Bot maju
            SetForward(180);
            // Belok kanan lebih tajam
            SetTurnRight(80);
            // Jalankan movement
            Go();

            // ZIG KIRI
            // Bot maju
            SetForward(180);
            // Belok kiri lagi
            SetTurnLeft(80);
            // Jalankan movement
            Go();
        }
    }

    // =========================
    // EVENT SAAT MUSUH TERLIHAT
    // =========================
    // Method ini akan terus dijalankan saat radar menemukan musuh
    public override void OnScannedBot(ScannedBotEvent e)
    {
        // =========================
        // HEURISTIK GREEDY
        // =========================
        // Jika musuh terlihat maka:
        // 1. Fokus penuh ke musuh
        // 2. Lock radar
        // 3. Lock gun
        // 4. Menghadap musuh
        // 5. Menembak damage maksimum
        // 6. Mendekati musuh

        // Menghitung jarak bot ke musuh
        double distance = DistanceTo(e.X, e.Y);

        // =========================
        // RADAR LOCK
        // =========================

        // Menghitung sudut radar ke musuh
        double radarTurn = DirectionTo(e.X, e.Y) - RadarDirection;

        // Radar diputar ke arah musuh
        SetTurnRadarLeft(NormalizeRelativeAngle(radarTurn));

        // =========
        // GUN LOCK
        // =========

        // Menghitung sudut gun ke musuh
        double gunTurn = DirectionTo(e.X, e.Y) - GunDirection;

        // Gun diputar ke arah musuh
        SetTurnGunLeft(NormalizeRelativeAngle(gunTurn));

        // ===========
        // BODY LOCK
        // ===========
        // Body ikut menghadap musuh supaya agresif melawan target

        // Menghitung arah body ke musuh
        double bodyTurn = DirectionTo(e.X, e.Y) - Direction;

        // Body diputar ke arah musuh
        SetTurnLeft(NormalizeRelativeAngle(bodyTurn));

        // =========================
        // HIGH DAMAGE FIRE
        // =========================
        // Jika gun hampir tepat ke musuh, langsung tembak dengan power maksimum

        // Jika sisa sudut gun kecil
        if (Math.Abs(GunTurnRemaining) < 8)
        {
            Fire(3); //Damage maksimum
        }

        // =================
        // MENDEKATI MUSUH
        // =================
        // Jika musuh jauh bot akan mendekati musuh
        if (DistanceTo(e.X, e.Y) > 150)
        {
            SetForward(120); //Maju ke arah musuh
        }
    }

    // =========================
    // EVENT KENA DINDING
    // =========================
    public override void OnHitWall(HitWallEvent e)
    {
        // Membalik arah gerakan
        ReverseDirection();
        // Belok supaya keluar dari dinding
        SetTurnRight(90);
        // Maju menjauh dari dinding
        SetForward(150);
        // Menjalankan movement
        Go();
    }

    // =========================
    // EVENT TABRAK MUSUH
    // =========================
    public override void OnHitBot(HitBotEvent e)
    {
        // Saat dekat musuh langsung tembak maksimum
        // Fire power maksimum
        Fire(3);

        // Tetap maju agar terus menekan lawan
        SetForward(100);
    }

    // =========================
    // EVENT KENA PELURU
    // =========================
    public override void OnHitByBullet(HitByBulletEvent e)
    {
        // =========================
        // DODGE ZIGZAG
        // =========================
        // Saat terkena peluru, bot mengubah arah secara cepat agar sulit ditembak

        // Belok kanan
        SetTurnRight(50);

        // Maju
        SetForward(100);

        // Jalankan movement
        Go();

        // Belok kiri tajam
        SetTurnLeft(100);

        // Maju lagi
        SetForward(100);

        // Jalankan movement
        Go();
    }

    // =========================
    // METHOD REVERSE DIRECTION
    // =========================
    // Method untuk membalik arah gerakan bot
    private void ReverseDirection()
    {
        // Jika sebelumnya bot maju
        if (movingForward)
        {
            // Bot mundur
            SetBack(150);

            // Status diubah menjadi mundur
            movingForward = false;
        }
        else
        {
            // Jika sebelumnya bot mundur
            // maka bot maju
            SetForward(150);

            // Status diubah menjadi maju
            movingForward = true;
        }
    }
}