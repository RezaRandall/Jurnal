namespace TabpediaFin.Handler.CoAHandler
{
    public class GenerateAccountInsertHandler : IRequestHandler<GenerateAccountInsertDto, PagedListResponse<AccountListDto>>
    {
        private readonly FinContext _context;
        private readonly DbManager _dbManager;
        private readonly ICurrentUser _currentUser;

        public GenerateAccountInsertHandler(FinContext db, DbManager dbManager, ICurrentUser currentUser)
        {
            _context = db;
            _dbManager = dbManager;
            _currentUser = currentUser;
        }

        public async Task<PagedListResponse<AccountListDto>> Handle(GenerateAccountInsertDto request, CancellationToken cancellationToken)
        {
            var result = new PagedListResponse<AccountListDto>();

            try
            {
                using (var cn = _dbManager.CreateConnection())
                {
                    var sqlcount = @$"Select count(1) from ""Account"" where ""TenantId"" = @TenantId";
                    var parameters = new DynamicParameters();
                    parameters.Add("TenantId", _currentUser.TenantId);
                    
                    var totalRecord = await cn.ExecuteScalarAsync<int>(sqlcount, parameters);
                    
                    if(totalRecord > 0)
                    {
                        result.IsOk = false;
                        result.ErrorMessage = "Account has already been generated";
                        return result;
                    }
                }
                List<Account> listofAccount = new List<Account>();
                listofAccount.AddRange(new List<Account>
                {
                    new Account("Kas", "1-10001", 1, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Rekening Bank", "1-10002", 1, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Giro", "1-10003", 1, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Piutang Usaha", "1-10100", 2, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Piutang Belum Ditagih", "1-10101", 2, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Cadangan Kerugian Piutang", "1-10102", 2, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Persediaan Barang", "1-10200", 3, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Piutang Lainnya", "1-10300", 4, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Piutang Karyawan", "1-10301", 4, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Dana Belum Disetor", "1-10400", 4, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Aset Lancar Lainnya", "1-10401", 4, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Biaya Dibayar Di Muka", "1-10402", 4, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Uang Muka", "1-10403", 4, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("PPN Masukan", "1-10500", 4, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pajak Dibayar Di Muka - PPh 22", "1-10501", 4, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pajak Dibayar Di Muka - PPh 23", "1-10502", 4, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pajak Dibayar Di Muka - PPh 25", "	1-10503", 4, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Aset Tetap - Tanah", "	1-10700", 5, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Aset Tetap - Bangunan", "1-10701", 5, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Aset Tetap - Building Improvements", "1-10702", 5, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Aset Tetap - Kendaraan", "1-10703", 5, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Aset Tetap - Mesin & Peralatan", "1-10704", 5, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Aset Tetap - Perlengkapan Kantor", "1-10705", 5, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Aset Tetap - Aset Sewa Guna Usaha", "1-10706", 5, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Aset Tak Berwujud", "1-10707", 5, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Akumulasi Penyusutan - Bangunan", "1-10751", 6, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Akumulasi Penyusutan - Building Improvements", "1-10752", 6, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Akumulasi penyusutan - Kendaraan", "1-10753", 6, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Akumulasi Penyusutan - Mesin & Peralatan", "1-10754", 6, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Akumulasi Penyusutan - Peralatan Kantor", "1-10755", 6, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Akumulasi Penyusutan - Aset Sewa Guna Usaha", "1-10756", 6, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Akumulasi Amortisasi", "1-10757", 6, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Investasi", "1-10800", 7, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Hutang Usaha", "2-20100", 8, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Hutang Belum Ditagih", "2-20101", 8, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Hutang Lain Lain", "2-20200", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Hutang Gaji", "2-20201", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Hutang Deviden", "2-20202", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pendapatan Diterima Di Muka", "2-20203", 9, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Sarana Kantor Terhutang", "2-20301", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Bunga Terhutang", "2-20302", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Biaya Terhutang Lainnya", "2-20399", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Hutang Bank", "2-20400", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("PPN Keluaran", "2-20500", 9, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Hutang Pajak - PPh 21", "2-20501", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Hutang Pajak - PPh 22", "2-20502", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Hutang Pajak - PPh 23", "2-20503", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Hutang Pajak - PPh 29", "2-20504", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Hutang Pajak Lainnya", "2-20599", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Hutang dari Pemegang Saham", "2-20600", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Kewajiban Lancar Lainnya", "2-20601", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Kewajiban Manfaat Karyawan", "2-20700", 9, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Modal Saham", "3-30000", 10, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Tambahan Modal Disetor", "3-30001", 10, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Laba Ditahan", "3-30100", 10, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Deviden", "3-30200", 10, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pendapatan Komprehensif Lainnya", "3-30300", 10, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Ekuitas Saldo Awal", "3-30399", 10, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pendapatan", "4-40000", 11, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Diskon Penjualan", "4-40100", 11, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Retur Penjualan", "4-40200", 11, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pendapatan Belum Ditagih", "4-40201", 11, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Beban Pokok Pendapatan", "5-50000", 12, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Diskon Pembelian", "5-50100", 12, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Retur Pembelian", "5-50200", 12, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pengiriman & Pengangkutan", "5-50300", 12, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Biaya Impor", "5-50400", 12, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Biaya Produksi", "5-50500", 12, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Biaya Penjualan", "6-60000", 13, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Biaya Umum & Administratif", "6-60100", 13, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pengeluaran Barang Rusak", "6-60216", 13, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pendapatan Bunga - Bank", "7-70000", 14, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pendapatan Bunga - Deposito", "7-70001", 14, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pembulatan", "7-70002", 14, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pendapatan Lain - lain", "7-70099", 14, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Beban Bunga", "8-80000", 15, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Provisi", "8-80001", 15, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("(Laba)/Rugi Pelepasan Aset Tetap", "8-80002", 15, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Penyesuaian Persediaan", "8-80100", 15, 0, 0, "", 0, true, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Beban Lain - lain", "8-80999", 15, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Beban Pajak - Kini", "9-90000", 15, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Beban Pajak - Tangguhan", "9-90001", 15, 0, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId)
                });

                await _context.Account.AddRangeAsync(listofAccount, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);


                int idparent = listofAccount[24].Id;
                List<Account> ChildofAccountAssetTakWujud = new List<Account>();
                ChildofAccountAssetTakWujud.AddRange(new List<Account>
                {
                    
                    new Account("Hak Merek Dagang", "1-10708", 5, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Hak Cipta", "1-10709", 5, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Good Will", "1-10710", 5, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                });

                idparent = listofAccount[31].Id;
                List<Account> ChildofAccountAmortisasi = new List<Account>();
                ChildofAccountAmortisasi.AddRange(new List<Account>
                {

                    new Account("Akumulasi Amortisasi : Hak Merek Dagang", "1-10708", 6, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Akumulasi Amortisasi : Hak Cipta", "1-10709", 6, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Akumulasi Amortisasi : Good Will", "1-10710", 6, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                });
                idparent = listofAccount[68].Id;
                List<Account> ChildofAccountPenjualan = new List<Account>();
                ChildofAccountPenjualan.AddRange(new List<Account>
                {
                    new Account("Iklan & Promosi", "6-60001", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Komisi & Fee", "6-60002", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Bensin, Tol dan Parkir - Penjualan", "6-60003", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Perjalanan Dinas - Penjualan", "6-60004", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Komunikasi - Penjualan", "6-60005", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Marketing Lainnya", "6-60006", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                });
                idparent = listofAccount[69].Id;
                List<Account> ChildofAccountbiayaumum = new List<Account>();
                ChildofAccountbiayaumum.AddRange(new List<Account>
                {
                    new Account("Gaji", "6-60101", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Upah", "6-60102", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Makanan & Transportasi", "6-60103", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Lembur", "6-60104", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pengobatan", "6-60105", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("THR & Bonus", "6-60106", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Jamsostek", "6-60107", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Insentif", "6-60108", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pesangon", "6-60109", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Manfaat dan Tunjangan Lain", "6-60110", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Donasi", "6-60200", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Hiburan", "6-60201", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Bensin, Tol dan Parkir - Umum", "6-60202", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Perbaikan & Pemeliharaan", "6-60203", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Perjalanan Dinas - Umum", "6-60204", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Makanan", "6-60205", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Komunikasi - Umum", "6-60206", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Iuran & Langganan", "6-60207", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Asuransi", "6-60208", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Legal & Profesional", "6-60209", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Beban Manfaat Karyawan", "6-60210", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Sarana Kantor", "6-60211", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pelatihan & Pengembangan", "6-60212", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Beban Piutang Tak Tertagih", "6-60213", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pajak dan Perizinan", "6-60214", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Denda", "6-60215", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Listrik", "6-60217", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Air", "6-60218", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("IPL", "6-60219", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Langganan Software", "6-60220", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Beban Kantor", "6-60300", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Alat Tulis Kantor & Printing", "6-60301", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Bea Materai", "6-60302", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Keamanan dan Kebersihan", "6-60303", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Supplies dan Material", "6-60304", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Pemborong", "6-60305", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Biaya Sewa - Bangunan", "6-60400", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Biaya Sewa - Kendaraan", "6-60401", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Biaya Sewa - Operasional", "6-60402", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Biaya Sewa - Lain - lain", "6-60403", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Penyusutan - Bangunan", "6-60500", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Penyusutan - Perbaikan Bangunan", "6-60501", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Penyusutan - Kendaraan", "6-60502", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Penyusutan - Mesin & Peralatan", "6-60503", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Penyusutan - Peralatan Kantor", "6-60504", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Penyusutan - Aset Sewa Guna Usaha", "6-60599", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Amortisasi : Hak Merek Dagang", "6-60600", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Amortisasi : Hak Cipta", "6-60601", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId),
                    new Account("Amortisasi : Good Will", "6-60602", 13, idparent, 0, "", 0, false, _currentUser.TenantId, _currentUser.UserId)
                });

                await _context.Account.AddRangeAsync(ChildofAccountAssetTakWujud, cancellationToken);
                await _context.Account.AddRangeAsync(ChildofAccountAmortisasi, cancellationToken);
                await _context.Account.AddRangeAsync(ChildofAccountPenjualan, cancellationToken);
                await _context.Account.AddRangeAsync(ChildofAccountbiayaumum, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                result.IsOk = true;
                result.ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                result.IsOk = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
    public class GenerateAccountInsertDto : IRequest<PagedListResponse<AccountListDto>>
    {
    }
}
