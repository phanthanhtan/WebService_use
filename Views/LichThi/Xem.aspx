<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Layout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--#include file="../Form.aspx"-->
    <div>
        <div align="center">
            <h2>Lịch thi cá nhân</h2>
            <!--#include file="../FormThongTin.aspx"-->
        </div>
            <%
            int check = Convert.ToInt32(ViewBag.Check);
            switch (check)
            {
                case 1://ViewBag.LichThi //xem tất cả học kỳ
                    int dem = 0;
                    foreach (WebService_use.Models.LichThi_Xem LT in (List<WebService_use.Models.LichThi_Xem>)ViewBag.LichThi)
                    {
                        if (dem == 0)
                        {%>
                        <div align="center">
                            <a href="/LichThi/CapNhat/<%=LT.MaSinhVien %>">Cập nhật lịch thi</a> - 
                            <a href="/LichThi/Xoa/<%=LT.MaSinhVien %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa lịch thi</a>
                            <table cellspacing="0" cellpadding="0" border="1" style="text-align: center">
                                <tr>
                                    <td>Mã học kỳ</td>
                                    <td>Tên học kỳ</td>
                                    <td>Số môn thi</td>
                                    <td colspan="3">Chức năng</td>
                                </tr>
                        <%dem = 1;
                        }%> 
                                <tr>
                                    <td><%=LT.MaHocKy %></td>
                                    <td><%=LT.TenHocKy %></td>
                                    <td><%=LT.SoMonThi %></td>
                                    <td><%if (LT.SoMonThi != 0){%><a href="/LichThi/Xem/<%=LT.MaSinhVien %>/<%=LT.MaHocKy %>">Xem</a><%}%></td>
                                    <td><a href="/LichThi/CapNhat/<%=LT.MaSinhVien %>/<%=LT.MaHocKy %>">Cập nhật</a></td>
                                    <td><a href="/LichThi/Xoa/<%=LT.MaSinhVien %>/<%=LT.MaHocKy %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa</a></td>
                                </tr>
                    <%}   %>
                                <tr></tr>
                            </table>
                        </div><%
                    break;
                case 2://ViewBag.LichThi //chi tiết theo mã học kỳ
                    int dem1 = 0;
                    foreach (WebService_use.Models.LichThi LTCT in (List<WebService_use.Models.LichThi>)ViewBag.LichThi)
                    {
                        if (dem1 == 0)
                        {%>
                        <div>
                        <table>
                            <tr>
                                <td>Mã học kỳ: </td>
                                <td><%=LTCT.MaHocKy %></td>
                            </tr>
                            <tr>
                                <td>Tên học kỳ: </td>
                                <td><%=LTCT.HocKy.TenHocKy %></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><a href="/LichThi/CapNhat/<%=LTCT.MaSinhVien %>/<%=LTCT.MaHocKy %>">Cập nhật</a> - 
                                    <a href="/LichThi/Xoa/<%=LTCT.MaSinhVien %>/<%=LTCT.MaHocKy %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa</a>
                                    <br /><a href="/LichThi/Xem/<%=LTCT.MaSinhVien %>">Xem tất cả học kỳ</a>
                                </td>
                            </tr>
                        </table>
                        <%if (LTCT.SoMonThi != 0)
                            {%>
                        <table cellspacing="0" cellpadding="0" border="1" style="text-align: center">
                            <tr>
                                <td>STT</td>
                                <td>Mã môn học</td>
                                <td>Tên môn học</td>
                                <td>Ghép thi</td>
                                <td>Tổ thi</td>
                                <td>Số lượng</td>
                                <td>Ngày thi</td>
                                <td>Giờ bắt đầu</td>
                                <td>Số phút</td>
                                <td>Phòng</td>
                                <td>Hình thức thi</td>
                            </tr>
                            <%}%>
                        <%dem1 = 1;
                        } if (LTCT.SoMonThi != 0)
                            {%>
                            <tr>
                                <td><%=LTCT.SoThuTu%></td>
                                <td><%=LTCT.MaMonHoc%></td>
                                <td style="text-align: left"><%=LTCT.MonHoc.TenMonHoc%></td>
                                <td><%=LTCT.GhepThi%></td>
                                <td><%=LTCT.ToThi%></td>
                                <td><%=LTCT.SoLuong%></td>
                                <td><%=LTCT.NgayThi%></td>
                                <td><%=LTCT.GioBatDau%></td>
                                <td><%=LTCT.SoPhut%></td>
                                <td><%=LTCT.Phong%></td>
                                <td><%=LTCT.HinhThucThi%></td>
                            </tr>
                            <%}
                            else Response.Write("<font color='red'>Không có lịch thi.</font>");
                            %>
                    <%}%>
                    </table>
                    </div>
                    <%
                    break;
                default:
                    %>
                    <script>alert('Có lỗi xảy ra trong quá trình xử lý. Vui lòng thao tác lại!');
                        window.location = "/LichThi/Index";</script>
                    <%
                    break;
            }
            String alert = Convert.ToString(ViewBag.Alert);
            switch (alert)
            {
                case "CapNhatOk":
                    %>
                    <script>alert('Cập nhật thành công.');</script>
                    <%
                    break;
                default:
                    break;
            }
        %>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#LichThi').addClass('active');
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
