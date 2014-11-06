<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Layout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--#include file="../Form.aspx"-->
    <div align="center">
        <h2>Xem điểm</h2>
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
                    <%foreach (WebService_use.Models.Select_Index dtq in (List<WebService_use.Models.Select_Index>)ViewBag.Diem_TongQuat)
                    {%>
                    <tr>
                        <td><%=dtq.MaSinhVien %></td>
                        <td style="text-align: left"><%=dtq.TenSinhVien %></td>
                        <td><a href="/Diem/Xem/<%=dtq.MaSinhVien %>">Xem</a></td>
                        <td><a href="/Diem/CapNhat/<%=dtq.MaSinhVien %>">Cập nhật</a></td>
                        <td><a href="/Diem/Xoa/<%=dtq.MaSinhVien %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa</a></td>
                    </tr>
                    <%}%>
                </table>
                <%
                    break;
                case "Index2":
                    Response.Write("Chưa có điểm thi nào trong cơ sở dữ liệu!");
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
                    <script>alert("Không có điểm thi cần xóa. Vui lòng kiểm tra lại mã sinh viên và mã học kỳ!");</script>
                    <%
                    break;
                case "XoaNull":
                    %>
                    <script>alert("Không có điểm thi của mã sinh viên cần xóa. Vui lòng kiểm tra lại!");</script>
                    <%
                    break;
                case "XoaError":
                    %>
                    <script>alert("Có lỗi xảy ra trong quá trình xóa. Vui lòng thao tác lại!");</script>
                    <%
                    break;
                case "XemNull":
                    %>
                    <script>alert("Không có điểm thi trong cơ sở dữ liệu. Vui lòng kiểm tra lại mã sinh viên và mã học kỳ!");</script>
                    <%
                    break;
                case "XemError":
                    %>
                    <script>alert("Có lỗi xảy ra trong quá trình lấy điểm thi. Vui lòng thao tác lại!");</script>
                    <%
                    break;
                case "CapNhatNull":
                    %>
                    <script>alert("Trang http://thongtindaotao.sgu.edu.vn cho biết không có mã sinh viên vừa nhập. Vui lòng kiểm tra lại!");</script>
                    <%
                    break;
                case "CapNhatNullMaHocKy":
                    %>
                    <script>alert("Không có điểm thi cần cập nhật. Vui lòng kiểm tra lại mã sinh viên và mã học kỳ!");</script>
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
            $('#Diem').addClass('active');
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
