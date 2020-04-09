# GZipTest

Program GZipTest (GzipTest.exe ve složce GzipTest) je spouštěn z příkazové řádky zadáním 3 parametrů: 
požadované operace (compress/decompress), vstupního souboru (plná cesta k souboru) a výstupního souboru 
(plná cesta k novému, výstupnímu souboru). Parametry jsou předány Main metodě ve třídě Program.

Samotná komprese/dekomprese je založena na vzoru producent–konzument, kdy producent představuje jedno vlákno, 
které načítá vstupní soubor po částech a vkládá ho do fronty. Konzument potom v několika vláknech načítá části 
z fronty, ve vláknech provádí požadovanou operaci a zapisuje do nového souboru. 

Producentské vlákno a konzumentská vlákna jsou spouštěna z třídy ProducerConsumer. 
Vlákna, umístěná pod společným zámkem, jsou synchronizována pomocí třídy Monitor a metod Wait a Pulse, 
které odemykají zámek pro jedno nebo druhé vlákno podle toho, jestli je fronta prázdná, nebo naplněná. 
Načítání dat ze souboru (původního i zkomprimovaného) a jejich zařazení do fronty řídí abstraktní třída Producer, 
z níž dědí třídy CompressingProducer a DecompressingProducer, které implementují konkrétní způsob načítání ze souboru. 
CompressingProducer načítá ze souboru pole bytů dané velikosti (blockSize), přiřazuje jim index a přidává je do objektu 
typu IndexedDataObject. Objekty potom zařazuje do fronty. DecompressingProducer načítá (deserializuje) ze souboru objekty 
typu IndexedDataObject a zařazuje je do fronty.

Vybírání dat z fronty řídí abstraktní třída Consumer, která poté data zpracovává a zapisuje do výstupního souboru. 
Konkrétní implementaci požadovaného zpracování dat a následného zápisu realizují třídy CompressingConsumer a DecompressingConsumer, 
které ze třídy Consumer dědí. CompressingConsumer zkomprimuje pole bytů uvnitř objektů (třída Compression) a poté objekty zapíše 
(serializuje; třída Serialization) do výstupního souboru. DecompressingConsumer dekomprimuje zkomprimovaná pole bytů uvnitř objektů 
(třída Decompression) a poté tato pole bytů zapíše do výstupního souboru. Správné pořadí zápisu zajišťuje třída Consumer, která 
ověřuje indexy uvnitř objektů a synchronizuje konzumentská vlákna pomocí metod Monitor.Wait a Monitor.PulseAll.

Případný vznik výjimek zachytává třída ErrorsChecker, která zaprvé ověřuje správný počet vstupních parametrů, 
včetně správnosti prvního parametru (požadované operace). Metodou SetError tato třída zachytává I výjimky vzniklé 
v producentských a konzumentských vláknech. Při zachycení výjimky vypíše konkrétní chybové hlášení a ukončení 
program s návratovým kódem 1 (chybový kód předán jako parametr metodě Environment.Exit).

Při úspěšném dokončení programu je návratový kód 0 (nastaveno v metodě Main).

Komentáře v programu i chybová hlášení jsou uvedeny v angličtině.

