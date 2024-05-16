-- MySQL dump 10.13  Distrib 8.0.36, for Linux (x86_64)
--
-- Host: localhost    Database: tubes3_stima24
-- ------------------------------------------------------
-- Server version	8.0.36-0ubuntu0.22.04.1

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `biodata`
--

DROP TABLE IF EXISTS `biodata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `biodata` (
  `NIK` varchar(16) NOT NULL,
  `nama` varchar(100) DEFAULT NULL,
  `tempat_lahir` varchar(50) DEFAULT NULL,
  `tanggal_lahir` date DEFAULT NULL,
  `jenis_kelamin` enum('Laki-Laki','Perempuan') DEFAULT NULL,
  `golongan_darah` varchar(5) DEFAULT NULL,
  `alamat` varchar(255) DEFAULT NULL,
  `agama` varchar(50) DEFAULT NULL,
  `status_perkawinan` enum('Belum Menikah','Menikah','Cerai') DEFAULT NULL,
  `pekerjaan` varchar(100) DEFAULT NULL,
  `kewarganegaraan` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`NIK`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `biodata`
--

LOCK TABLES `biodata` WRITE;
/*!40000 ALTER TABLE `biodata` DISABLE KEYS */;
('1234567890123456', 'John Doe', 'Jakarta', '1990-01-01', 'Laki-Laki', 'A', 'Jl. ABC No. 123', 'Islam', 'Belum Menikah', 'PNS', 'WNI'),
('2345678901234567', 'Jane Smith', 'Surabaya', '1992-05-15', 'Perempuan', 'O', 'Jl. XYZ No. 456', 'Kristen', 'Menikah', 'Dokter', 'WNI'),
('3456789012345678', 'Alice Johnson', 'Bandung', '1985-09-30', 'Perempuan', 'B', 'Jl. DEF No. 789', 'Katolik', 'Cerai', 'Guru', 'WNI'),
('4567890123456789', 'Bob Brown', 'Yogyakarta', '1988-12-25', 'Laki-Laki', 'AB', 'Jl. GHI No. 012', 'Buddha', 'Menikah', 'Arsitek', 'WNI'),
('5678901234567890', 'Eva Martinez', 'Semarang', '1995-03-18', 'Perempuan', 'O', 'Jl. MNO No. 345', 'Islam', 'Belum Menikah', 'Penulis', 'WNI'),
('6789012345678901', 'Michael Johnson', 'Malang', '1993-07-22', 'Laki-Laki', 'A', 'Jl. PQR No. 678', 'Protestan', 'Cerai', 'Pengacara', 'WNI'),
('7890123456789012', 'Sarah Lee', 'Medan', '1987-11-10', 'Perempuan', 'AB', 'Jl. STU No. 901', 'Buddha', 'Menikah', 'Akuntan', 'WNI'),
('8901234567890123', 'David Wang', 'Denpasar', '1991-04-05', 'Laki-Laki', 'B', 'Jl. VWX No. 234', 'Kristen', 'Belum Menikah', 'Dosen'),
('9012345678901234', 'Sophia Chen', 'Balikpapan', '1986-08-20', 'Perempuan', 'O', 'Jl. YZA No. 567', 'Hindu', 'Cerai', 'Psikolog', 'WNI'),
('0123456789012345', 'Chris Liu', 'Pontianak', '1994-06-28', 'Laki-Laki', 'A', 'Jl. ABC No. 890', 'Islam', 'Menikah', 'Insinyur', 'WNI');
/*!40000 ALTER TABLE `biodata` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sidik_jari`
--

DROP TABLE IF EXISTS `sidik_jari`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sidik_jari` (
  `berkas_citra` text,
  `nama` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sidik_jari`
--

LOCK TABLES `sidik_jari` WRITE;
/*!40000 ALTER TABLE `sidik_jari` DISABLE KEYS */;
/*!40000 ALTER TABLE `sidik_jari` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-05-04 15:57:34
