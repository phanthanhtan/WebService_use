<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Layout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--#include file="../FormMonHoc.aspx"-->
    <div align="center">
        <h2>Môn học - Chi tiết</h2>
        <%  WebService_use.Models.SXTKB_MonHoc mh = (WebService_use.Models.SXTKB_MonHoc)ViewBag.SXTKB_MonHoc;
            %>
            <table>
                <tr>
                    <td colspan="2"><form action="/SXTKB_MonHoc/ThemNhomMonHoc" method="post" id="FNhomMonHoc" name="FNhomMonHoc" onsubmit = "return CheckFormNhomMonHoc()"></td>
                </tr>
                <tr>
                    <td>Mã môn học:</td><td><input id="txtMaMonHoc" name="txtMaMonHoc" value="<%=mh.MaMonHoc %>" readonly="true"/></td>
                </tr>
                <tr>
                    <td>Tên môn học:</td><td><input id="txtTenMonHoc" name="txtTenMonHoc" value="<%=mh.TenMonHoc %>" readonly="true"/></td>
                </tr>
                <tr>
                    <td>Nhóm:</td><td><input id="txtNhom" name="txtNhom" onkeypress="return CheckNumber(event)"/></td>
                </tr>
                <tr>
                    <td></td><td>Nhóm nhập các ký tự: 0-9</td>
                </tr>
                <tr>
                    <td>Thứ:</td><td><input id="txtThu" name="txtThu" onkeypress="return CheckNumber(event)"/></td>
                </tr>
                <tr>
                    <td></td><td>Thứ: 2 3 4 5 6 7<br />Chủ nhật: 8</td>
                </tr>
                <tr>
                    <td>Tiết bắt đầu:</td><td><input id="txtTietBatDau" name="txtTietBatDau" onkeypress="return CheckNumber(event)"/></td>
                </tr>
                <tr>
                    <td></td><td>1 2 3 4 5 6 7 8 9 10 11 12 13</td>
                </tr>
                <tr>
                    <td>Số tiết:</td><td><input id="txtSoTiet" name="txtSoTiet" onkeypress="return CheckNumber(event)"/></td>
                </tr>
                <tr>
                    <td></td><td>Tiết bắt đầu + Số tiết < 15</td>
                </tr>
                <tr>
                    <td></td><td><input type="submit" id="Them" name="Them" value="Thêm nhóm môn học" /></td>
                </tr>
                <tr><td></form></td></tr>
                <tr>
                    <td></td><td><a href="/SXTKB/SXTKB_MonHoc/XoaMonHoc/<%=mh.MaMonHoc %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa môn học</a></td>
                </tr>
            </table>
            <%
            List<WebService_use.Models.SXTKB_NhomMonHoc> ListNhomMonHoc = (List<WebService_use.Models.SXTKB_NhomMonHoc>)ViewBag.SXTKB_NhomMonHoc;
            if (ListNhomMonHoc.Count() != 0)
            {
                int dem = 0;
                foreach (WebService_use.Models.SXTKB_NhomMonHoc Nmh in ListNhomMonHoc)
                {
                    if (dem == 0)
                    {%>
                        <br />
                        <table cellspacing="0" cellpadding="0" border="1" style="text-align: center">
                            <tr>
                                <td>Nhóm</td><td>Thứ</td><td>Tiết bắt đầu</td><td>Số tiết</td>
                                <td>Chức năng</td>
                            </tr>
                    <%dem = 1;
                    }%>
                            <tr>
                                <td><%=Nmh.Nhom %></td><td><%=Nmh.Thu %></td><td><%=Nmh.TietBatDau %></td><td><%=Nmh.SoTiet %></td>
                                <td><a href="/SXTKB/SXTKB_MonHoc/XoaNhomMonHoc/<%=Nmh.MaMonHoc %>/<%=Nmh.Nhom %>/<%=Nmh.Thu %>/<%=Nmh.TietBatDau %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa</a></td>
                            </tr>
                <%}%>
                        </table>
            <%}
            else
                Response.Write("Chưa có nhóm môn học.");
            %>
    <%  String alert = Convert.ToString(ViewBag.Alert);
        switch (alert)
        {
            case "ThemMonHocOk":
                %>
                <script>alert('Thêm môn học thành công.');</script>
                <%
                break;
            case "ThemNhomMonHocOk":
                %>
                <script>alert('Thêm nhóm môn học thành công.');</script>
                <%
                break;
            case "ThemNhomMonHocErrorCheck":
                %>
                <script>alert('Có dữ liệu (thứ, tiết bắt đầu, số tiết) không hợp lệ. Vui lòng thao tác lại!');</script>
                <%
                break;
            case "ThemNhomMonHocError":
                %>
                <script>alert('Có lỗi xảy ra trong quá trình thêm nhóm môn học. Vui lòng thao tác lại!');</script>
                <%
                break;
            case "XoaNhomMonHocOk":
                %>
                <script>alert('Xóa nhóm môn học thành công.');</script>
                <%
                break;
            case "XoaNhomMonHocNull":
                %>
                <script>alert('Không có nhóm môn học cần xóa. Vui lòng kiểm tra lại!');</script>
                <%
                break;
            case "XoaNhomMonHocError":
                %>
                <script>alert('Có lỗi xảy ra trong quá trình xóa nhóm môn học. Vui lòng thao tác lại!');</script>
                <%
                break;
            case "SuaMonHocOk":
                %>
                <script>alert('Sửa môn học thành công.');</script>
                <%
                break;
            default:
                break;
        }
    %>
    </div>
    <script>
        function CheckFormNhomMonHoc() {
            if (txtNhom.value == "" || txtNhom.value == null) {
                alert("Chưa nhập nhóm!");
                txtNhom.focus();
                return false;
            }
            if (txtThu.value == "" || txtThu.value == null) {
                alert("Chưa nhập thứ!");
                txtThu.focus();
                return false;
            }
            if (txtTietBatDau.value == "" || txtTietBatDau.value == null) {
                alert("Chưa nhập tiết bắt đầu!");
                txtTietBatDau.focus();
                return false;
            }
            if (txtSoTiet.value == "" || txtSoTiet.value == null) {
                alert("Chưa nhập số tiết!");
                txtSoTiet.focus();
                return false;
            }
            if (parseInt(txtThu.value) < 2 || parseInt(txtThu.value) > 8) {
                alert("Thứ không hợp lệ!");
                txtThu.focus();
                return false;
            }
            if (parseInt(txtTietBatDau.value) <1 || parseInt(txtTietBatDau.value) > 13) {
                alert("Tiết bắt đầu không hợp lệ!");
                txtTietBatDau.focus();
                return false;
            }
            if (parseInt(txtSoTiet.value) < 1) {
                alert("Số tiết không hợp lệ!");
                txtSoTiet.focus();
                return false;
            }
            if (parseInt(txtSoTiet.value) > (14 - parseInt(txtTietBatDau.value))) {
                alert("Số tiết không hợp lệ!");
                txtSoTiet.focus();
                return false;
            }
        }
        function CheckNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
                            </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#MonHoc').addClass('active');
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
