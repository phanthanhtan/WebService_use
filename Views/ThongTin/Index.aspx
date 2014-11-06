<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Layout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<!--#include file="../Form.aspx"-->
<div align="center">
<h2>Thông tin cá nhân</h2>
<%  String ok = Convert.ToString(ViewBag.Ok);
    switch (ok)
    {
        case "Index1":
            %>
            <table cellspacing="0" cellpadding="0" border="1" style="text-align:center">
                <tr>
                    <td>Mã sinh viên</td><td>Tên sinh viên</td><td colspan="3">Chức năng</td>
                </tr>
            <%foreach (WebService_use.Models.Select_Index tk in (List<WebService_use.Models.Select_Index>)ViewBag.dsTK)
            {%>
            <tr>
                <td><%=tk.MaSinhVien %></td><td style="text-align:left"><%=tk.TenSinhVien %></td>
                <td><a href="/ThongTin/Xem/<%=tk.MaSinhVien %>">Xem</a></td>
                <td><a href="/ThongTin/CapNhat/<%=tk.MaSinhVien %>" >Cập nhật</a></td>
                <td><a href="/ThongTin/Xoa/<%=tk.MaSinhVien %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa</a></td>
            </tr>
            <%}%>
            </table>
            <%
            break;
        case "Index2":
            Response.Write("Chưa có thông tin cá nhân nào trong cơ sở dữ liệu!");
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
        case "XemNull":
            %>
            <script>alert("Chưa có thông tin cá nhân của mã sinh viên vừa nhập.");</script>
            <%
            break;
        case "XemError":
            %>
            <script>alert("Có lỗi xảy ra trong quá trình lấy thông tin cá nhân. Vui lòng thao tác lại!");</script>
            <%
            break;
        case "XoaOk":
            %>
            <script>alert("Xóa thành công.");</script>
            <%
            break;
        case "XoaLink":
            %>
            <script>alert("Thông tin tài khoản cần sử dụng ở mục khác. Nên không thể xóa. Vui lòng kiểm tra lại!");</script>
            <%
            break;
        case "XoaNull":
            %>
            <script>alert("Không có thông tin cá nhân cần xóa. Vui lòng kiểm tra lại!");</script>
            <%
            break;
        case "XoaEror":
            %>
            <script>alert("Có lỗi xảy ra trong quá trình xóa. Vui lòng thao tác lại!");</script>
            <%
            break;
        case "CapNhatError":
            %>
            <script>alert("Có lỗi xảy ra trong quá trình cập nhật. Vui lòng thao tác lại!");</script>
            <%
            break;
        case "CapNhatNull":
            %>
            <script>alert("Trang http://thongtindaotao.sgu.edu.vn cho biết không có mã sinh viên vừa nhập. Vui lòng kiểm tra lại!");</script>
            <%
            break;
        default:
            break;
    }
%>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#ThongTin').addClass('active');
    });
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
