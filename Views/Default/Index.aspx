<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Layout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center">
        <table style="text-align: left;">
            <tr>
                <td>Đây là đồ án môn Web Service
                <br />
                    <br />
                    Sinh viên thực hiện:
                <br />
                    Phan Thanh Tân
                <br />
                    Nguyễn Hoàng Khoa
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#TrangChu').addClass('active');
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
