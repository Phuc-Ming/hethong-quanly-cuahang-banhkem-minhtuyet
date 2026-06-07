using MySqlConnector.MySql;

namespace MinhTuyetBakery.DuLieu;

public class KetNoiCSDL
{
    private readonly string _chuoiKetNoi;

    public KetNoiCSDL(IConfiguration configuration)
    {
        _chuoiKetNoi = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Chưa cấu hình chuỗi kết nối.");
    }

    public MySqlConnection TaoKetNoi()
    {
        return new MySqlConnection(_chuoiKetNoi);
    }
}
