<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Layout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--#include file="../Form.aspx"-->
    <div align="center">
        <h2>Thời khóa biểu</h2>
        <%  String ok = Convert.ToString(ViewBag.Ok);
            switch (ok)
            {
                case "Index1"://load index
                %>
                <table cellspacing="0" cellpadding="0" border="1" style="text-align: center">
                    <tr>
                        <td>Mã sinh viên</td>
                        <td>Tên sinh viên</td>
                        <td colspan="3">Chức năng</td>
                    </tr>
                    <%foreach (WebService_use.Models.Select_Index TKB in (List<WebService_use.Models.Select_Index>)ViewBag.ThoiKhoaBieu)
                    {%>
                    <tr>
                        <td><%=TKB.MaSinhVien %></td>
                        <td style="text-align: left"><%=TKB.TenSinhVien %></td>
                        <td><a href="/ThoiKhoaBieu/Xem/<%=TKB.MaSinhVien %>">Xem</a></td>
                        <td><a href="/ThoiKhoaBieu/CapNhat/<%=TKB.MaSinhVien %>">Cập nhật</a></td>
                        <td><a href="/ThoiKhoaBieu/Xoa/<%=TKB.MaSinhVien %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa</a></td>
                    </tr>
                    <%}%>
                </table>
                <%
                    break;
                case "Index2":
                    Response.Write("Không có thời khóa biểu nào trong cơ sở dữ liệu.");
                    break;
                default:
                    Response.Write("Có lỗi xảy ra trong quá trình xử lý. Vui lòng thao tác lại!");
                    break;
            }
            String alert = Convert.ToString(ViewBag.Alert);
            switch (alert)
            {
                case "SubmitError":
                    %>
                    <script>alert("Có lỗi trong quá trình xử lý. Vui lòng thao tác lại!");</script>
                    <%
                    break;
                case "XoaOk":
                    %>
                    <script>alert("Xóa thành công.");</script>
                    <%
                    break;
                case "XoaNullMaHocKy":
                    %>
                    <script>alert("Không có lịch thi cần xóa. Vui lòng kiểm tra lại mã sinh viên và mã học kỳ!");</script>
                    <%
                    break;
                case "XoaNull":
                    %>
                    <script>alert("Không có lịch thi của mã sinh viên cần xóa. Vui lòng kiểm tra lại!");</script>
                    <%
                    break;
                case "XoaError":
                    %>
                    <script>alert("Có lỗi xảy ra trong quá trình xóa. Vui lòng thao tác lại!");</script>
                    <%
                    break;
                case "XemNull":
                    %>
                    <script>alert("Không có thời khóa biểu trong cơ sở dữ liệu. Vui lòng kiểm tra lại mã sinh viên và mã học kỳ!");</script>
                    <%
                    break;
                case "XemError":
                    %>
                    <script>alert("Có lỗi xảy ra trong quá trình lấy thông tin cá nhân. Vui lòng thao tác lại!");</script>
                    <%
                    break;
                case "CapNhatNull":
                    %>
                    <script>alert("Trang http://thongtindaotao.sgu.edu.vn cho biết không có mã sinh viên vừa nhập. Vui lòng kiểm tra lại!");</script>
                    <%
                    break;
                case "CapNhatNullMaHocKy":
                    %>
                    <script>alert("Không có thời khóa biểu cần cập nhật. Vui lòng kiểm tra lại mã sinh viên và mã học kỳ!");</script>
                    <%
                    break;
                case "CapNhatError":
                    %>
                    <script>alert("Có lỗi xảy ra trong quá trình cập nhật. Vui lòng thao tác lại!");</script>
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
