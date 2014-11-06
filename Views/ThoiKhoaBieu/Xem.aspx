<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Layout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--#include file="../Form.aspx"-->
    <div>
        <div align="center">
            <h2>Thời khóa biểu cá nhân</h2>
            <!--#include file="../FormThongTin.aspx"-->
        </div>
        <%  int check = Convert.ToInt32(ViewBag.Check);
            switch (check)
            {
                case 1://ViewBag.ThoiKhoaBieu
                    int dem = 0;
                    foreach (WebService_use.Models.ThoiKhoaBieu_Xem TKB in (List<WebService_use.Models.ThoiKhoaBieu_Xem>)ViewBag.ThoiKhoaBieu)
                    {
                        if (dem == 0)
                        {%>
                        <div align="center">
                            <a href="/ThoiKhoaBieu/CapNhat/<%=TKB.MaSinhVien %>">Cập nhật thời khóa biểu</a> - 
                                        <a href="/ThoiKhoaBieu/Xoa/<%=TKB.MaSinhVien %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa thời khóa biểu</a>
                            <table cellspacing="0" cellpadding="0" border="1" style="text-align: center">
                                <tr>
                                    <td>Mã học kỳ</td>
                                    <td>Tên học kỳ</td>
                                    <td colspan="3">Chức năng</td>
                                </tr>
                        <%} dem = 1;%> 
                                <tr>
                                    <td><%=TKB.MaHocKy %></td>
                                    <td><%=TKB.TenHocKy %></td>
                                    <td><a href="/ThoiKhoaBieu/Xem/<%=TKB.MaSinhVien %>/<%=TKB.MaHocKy %>">Xem</a></td>
                                    <td><a href="/ThoiKhoaBieu/CapNhat/<%=TKB.MaSinhVien %>/<%=TKB.MaHocKy %>">Cập nhật</a></td>
                                    <td><a href="/ThoiKhoaBieu/Xoa/<%=TKB.MaSinhVien %>/<%=TKB.MaHocKy %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa</a></td>
                                </tr>
                    <%}   %></table>
                        </div><%
                    break;
                case 2://chi tiết theo mã học kỳ ThoiKhoaBieu
                    int dem1 = 0;
                    foreach (WebService_use.Models.ThoiKhoaBieu_Xem_MaHocKy TKB in (List<WebService_use.Models.ThoiKhoaBieu_Xem_MaHocKy>)ViewBag.ThoiKhoaBieu)
                    {
                        if (dem1 == 0)
                        {%>
                        <div>
                        <table>
                            <tr>
                                <td class="text-align-right">Mã học kỳ: </td>
                                <td><%=TKB.MaHocKy %></td>
                            </tr>
                            <tr>
                                <td class="text-align-right">Tên học kỳ: </td>
                                <td><%=TKB.TenHocKy %></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><a href="/ThoiKhoaBieu/CapNhat/<%=TKB.MaSinhVien %>/<%=TKB.MaHocKy %>">Cập nhật</a> - 
                                    <a href="/ThoiKhoaBieu/Xoa/<%=TKB.MaSinhVien %>/<%=TKB.MaHocKy %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa</a>
                                    <br /><a href="/ThoiKhoaBieu/Xem/<%=TKB.MaSinhVien %>">Xem danh sách học kỳ</a>
                                </td>
                            </tr>
                        </table>
                        <select id="_MaTuan" onchange="return CheckTuan('/ThoiKhoaBieu/Xem/<%=TKB.MaSinhVien %>/<%=TKB.MaHocKy %>/');">
                        <%dem1 = 1;
                        }%>
                            <option id="<%=TKB.MaTuan %>" value="<%=TKB.MaTuan %>" <%if (ViewBag.MaTuan == TKB.MaTuan) {%>selected<%} %>><%=TKB.TenTuan %></option>
                    <%}%>
                        </select>
                        <table cellspacing="0" cellpadding="0" border="1" style="text-align: center">
                        <%if (dem1 == 1)
                            {%>
                            <tr>
                                <td>Thứ</td><td>Tiết bắt đầu</td><td>Số tiết</td>
                                <td>Mã môn học</td><td>Tên môn học</td><td>Phòng học</td>
                                <td>Giảng viên</td><td>Lớp</td>
                            </tr>  
                            <%
                            dem1 = 2;
                            }//ViewBag.ThoiKhoaBieu_Tuan
                            int dem_Tuan = 0;//kiểm tra trogn tuần có học ngày nào không
                            foreach (WebService_use.Models.ThoiKhoaBieu TKB_Tuan in (List<WebService_use.Models.ThoiKhoaBieu>)ViewBag.ThoiKhoaBieu_Tuan)
                            {
                            if (TKB_Tuan.Thu != "0")
                            {%>
                            <tr>
                                <td><%=TKB_Tuan.Thu %></td><td><%=TKB_Tuan.TietBatDau %></td><td><%=TKB_Tuan.SoTiet %></td>
                                <td><%=TKB_Tuan.MaMonHoc %></td><td><%=TKB_Tuan.MonHoc.TenMonHoc %></td><td><%=TKB_Tuan.PhongHoc %></td>
                                <td><%=TKB_Tuan.GiangVien %></td><td><%=TKB_Tuan.Lop %></td>
                            </tr>    
                            <%dem_Tuan++;
                            }
                            }
                            if (dem_Tuan == 0)
                            {%>
                            <tr>
                                <td colspan="8" style="color:red">Không có lịch học.</td>
                            </tr>
                            <%}%>
                        </table>
                        </div><%
                    break;
                default:
                    %>
                    <script>alert('Có lỗi xảy ra trong quá trình xử lý. Vui lòng thao tác lại!');
                        window.location = "/ThoiKhoaBieu/Index";</script>
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
            $('#ThoiKhoaBieu').addClass('active');
        });
    </script>
    <script>
        function CheckTuan(link) {
            //alert(link + _MaTuan.value.toString());
            window.location = link + _MaTuan.value.toString();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
