<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Layout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--#include file="../FormMonHoc.aspx"-->
    <div align="center">
        <h2>Môn học cần sắp xếp thời khóa biểu</h2>
        <%  String ok = Convert.ToString(ViewBag.Ok);
            switch (ok)
            {
                case "Index1"://load index ViewBag.SXTKB_MonHoc
                    %>
                    <table cellspacing="0" cellpadding="0" border="1" style="text-align: center">
                        <tr>
                            <td>Mã môn học</td>
                            <td>Tên môn học</td>
                            <td colspan="3">Chức năng</td>
                        </tr>
                        <%foreach (WebService_use.Models.SXTKB_MonHoc mh in (List<WebService_use.Models.SXTKB_MonHoc>)ViewBag.SXTKB_MonHoc)
                          {%>
                        <tr>
                            <td><%=mh.MaMonHoc %></td>
                            <td style="text-align: left"><%=mh.TenMonHoc %></td>
                            <td><a href="/SXTKB/SXTKB_MonHoc/XemMonHoc/<%=mh.MaMonHoc %>">Xem</a></td>
                            <td><a href="/SXTKB/SXTKB_MonHoc/SuaMonHoc/<%=mh.MaMonHoc %>">Sửa</a></td>
                            <td><a href="/SXTKB/SXTKB_MonHoc/XoaMonHoc/<%=mh.MaMonHoc %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa</a></td>
                        </tr>
                        <%}%>
                    </table>
                    <%
                    break;
                case "Index2":
                    Response.Write("Không có môn học nào trong cơ sở dữ liệu.");
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
                    <script>alert("Không có dữ liệu môn học trong cơ sở dữ liệu. Vui lòng kiểm tra lại!");</script>
                    <%
                    break;
                case "XemError":
                    %>
                    <script>alert("Có lỗi xảy ra trong quá trình lấy dữ liệu môn học. Vui lòng thao tác lại!");</script>
                    <%
                    break;
                case "XoaOk":
                    %>
                    <script>alert("Xóa môn học thành công.");</script>
                    <%
                    break;
                case "XoaNull":
                    %>
                    <script>alert("Không có môn học cần xóa. Vui lòng kiểm tra lại!");</script>
                    <%
                    break;
                case "XoaError":
                    %>
                    <script>alert("Có lỗi xảy ra trong quá trình xóa môn học. Vui lòng thao tác lại!");</script>
                    <%
                    break;
                case "ThemMonHocError":
                    %>
                    <script>alert("Có lỗi xảy ra trong quá trình thêm môn học. Vui lòng thao tác lại!");</script>
                    <%
                    break;
                case "SuaError"://lấy thông tin môn học => chuyển wa form sửa lỗi
                    %>
                    <script>alert("Có lỗi xảy ra trong quá trình xử lý. Vui lòng thao tác lại!");</script>
                    <%
                    break;
                case "SuaMonHocError":
                    %>
                    <script>alert("Có lỗi xảy ra trong quá trình sửa môn học. Vui lòng thao tác lại!");</script>
                    <%
                    break;
                default:
                    break;
            }
        %>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#MonHoc').addClass('active');
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
