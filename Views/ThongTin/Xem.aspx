<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Layout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<!--#include file="../Form.aspx"-->
<div align="center">
<h2>Thông tin cá nhân</h2>
<%int ok = Convert.ToInt32(ViewBag.Ok);
  switch (ok)
  {
      case 1:
        case 5:
          if (ok == 5)
          {%>
            <script>alert('Cập nhật thông tin thành công.');</script>
          <%}
          WebService_use.Models.ThongTin tt = (WebService_use.Models.ThongTin)ViewBag.strThongTin;
          %>
            <table>
                <tr><td class="text-align-right">Mã sinh viên:</td><td><%=tt.MaSinhVien %></td></tr>
                <tr><td class="text-align-right">Tên sinh viên:</td><td><%=tt.TenSinhVien %></td></tr>
                <tr><td class="text-align-right">Lớp:</td><td><%=tt.Lop %></td></tr>
                <tr><td class="text-align-right">Ngành:</td><td><%=tt.Nganh.TenNganh %></td></tr>
                <tr><td class="text-align-right">Khoa:</td><td><%=tt.Khoa.TenKhoa %></td></tr>
                <tr><td class="text-align-right">Hệ đào tạo:</td><td><%=tt.HeDaoTao.TenHeDaoTao %></td></tr>
                <tr><td class="text-align-right">Khóa học:</td><td><%=tt.KhoaHoc.TenKhoaHoc %></td></tr>
                <tr>
                    <td></td>
                    <td><a href="/ThongTin/CapNhat/<%=tt.MaSinhVien %>">Cập nhật</a>
                        <a href="/ThongTin/Xoa/<%=tt.MaSinhVien %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa</a></td>
                </tr>
            </table>
          <%
          break;
      default://Hệ thống xảy ra lỗi. Vui lòng thao tác lại!
        %>
        <script>alert('Có lỗi xảy ra trong quá trình xử lý. Vui lòng thao tác lại!');
        window.location = "/ThongTin/Index";</script>
        <%
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
