<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Layout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center">
        <%  String action = "ThemMonHocForm";
            WebService_use.Models.SXTKB_MonHoc mh = new WebService_use.Models.SXTKB_MonHoc();
            if (ViewBag.Action == "Sua")
            {
                mh = (WebService_use.Models.SXTKB_MonHoc)ViewBag.SXTKB_MonHoc;
                action = "SuaMonHocForm";
            }
        %>
        <h2>Môn học - Thêm</h2>
        <form action="/SXTKB_MonHoc/<%=action %>" method="post" id="FMonHoc" name="FMonHoc" onsubmit = "return CheckForm()">
            <table>
                <tr>
                    <td>Mã môn học:</td><td><input id="txtMaMonHoc" name="txtMaMonHoc" <%if (ViewBag.Action == "Sua") {%> value="<%=mh.MaMonHoc%>" readonly="true" <%}%>/></td>
                </tr>
                <tr>
                    <td>Tên môn học:</td><td><input id="txtTenMonHoc" name="txtTenMonHoc" <%if (ViewBag.Action == "Sua") {%> value="<%=mh.TenMonHoc%>"<%}%>/></td>
                </tr>
                <tr>
                    <td></td><td><input type="submit" id="Them" name="Them" <%if (ViewBag.Action == "Sua") {%>value="Sửa"<%} else {%>value="Thêm"<%}%> /></td>
                </tr>
            </table>
        </form>
    </div>
    <script>
        function CheckForm() {
            if (txtMaMonHoc.value == "" || txtMaMonHoc.value == null) {
                alert("Chưa nhập mã môn học!");
                txtMaMonHoc.focus();
                return false
            }
            if (txtTenMonHoc.value == "" || txtTenMonHoc.value == null) {
                alert("Chưa nhập tên môn học!");
                txtTenMonHoc.focus();
                return false
            }         
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
