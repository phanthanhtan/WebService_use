<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Layout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <div align="center">
            <!--#include file="../FormMonHoc.aspx"-->
        <%  String ok = Convert.ToString(ViewBag.Ok);
            switch (ok)
            {
                case "Index1"://có dữ liệu ViewBag.MonHoc
                    %>
                    <h2>Sắp xếp thời khóa biểu</h2>
                    <form action="/SapXepThoiKhoaBieu/CheckBox" method="post" id="FCheckBox" name="FCheckBox" onsubmit = "return CheckForm()">
                        <table cellspacing="0" cellpadding="0" border="1" style="text-align:center">
                            <tr>
                                <td>Ngày bận</td><td>Môn học</td>
                            </tr>
                            <tr>
                                <td style="text-align:left; vertical-align:top">
                                    <input type="checkbox" id="Ngay2" name="Ngay" value="2"/> Thứ 2<br />
                                    <input type="checkbox" id="Ngay3" name="Ngay" value="3"/> Thứ 3<br />
                                    <input type="checkbox" id="Ngay4" name="Ngay" value="4"/> Thứ 4<br />
                                    <input type="checkbox" id="Ngay5" name="Ngay" value="5"/> Thứ 5<br />
                                    <input type="checkbox" id="Ngay6" name="Ngay" value="6"/> Thứ 6<br />
                                    <input type="checkbox" id="Ngay7" name="Ngay" value="7"/> Thứ 7<br />
                                    <input type="checkbox" id="Ngay8" name="Ngay" value="8"/> Chủ nhật<br />
                                </td>
                                <td style="text-align:left; vertical-align:top">
                                <%  foreach (WebService_use.Models.SXTKB_MonHoc mh in (List<WebService_use.Models.SXTKB_MonHoc>)ViewBag.SXTKB_MonHoc)
                                    {%>
                                        <input type="checkbox" id="MonHoc" name="MonHoc" value="<%=mh.MaMonHoc %>"/> <%=mh.MaMonHoc %> - <%=mh.TenMonHoc %><br />
                                    <%}
                                %>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><input type="submit" id="Submit" name="Submit" value="Ok" /></td>
                            </tr>
                        </table>
                    </form>
                    <%
                    break;
                case "Index2"://không có dữ liệu
                    Response.Write("Không có dữ liệu môn học. Vui lòng thêm môn học!");
                    break;
                case "CheckBox1"://ok có TKB
                    %>  <h2>Các thời khóa biểu phù hợp</h2> <%
                        int dem = 0;
                        int stt = 1;
                        foreach (String tkb in (List<String>)ViewBag.ListTKB)
                        {
                            if (dem == 0)
                            {%>
                                <table cellspacing="0" cellpadding="0" border="1" style="text-align:center">
                                    <tr><td>STT</td><td>Mã môn học - nhóm</td></tr>
                                <%dem = 1;
                            }%>
                                    <tr><td><%=stt %></td><td><%=tkb %></td></tr>
                            <%stt++;
                        }
                        %>      </table>
                        <br />Bạn muốn sắp xếp thời khóa biểu mới, bấm vào <a href="/SapXepThoiKhoaBieu/Index">đây</a>.
                    <%
                    break;
                case "CheckBox2"://Không sắp đc TKB
                    %>
                    <script>alert("Không có thời khóa biểu nào phù hợp. Vui lòng chọn lại!");
                        window.location = "/SapXepThoiKhoaBieu/Index";</script>
                    <%
                    break;
                case "CheckBox3"://có môn học trùng ngày bận => không sắp đc TKB
                    %>
                    <script>alert("Có môn học (tất cả các nhóm) trùng ngày bận nên không thể sắp xếp thời khóa biểu.");
                        window.location = "/SapXepThoiKhoaBieu/Index";</script>
                    <%
                    break;
                case "CheckBox4"://chưa chọn môn học
                    %>
                    <script>alert("Bạn chưa chọn môn học nào nên không thể sắp xếp thời khóa biểu.");
                        window.location = "/SapXepThoiKhoaBieu/Index";</script>
                    <%
                    break;
                default:
                    Response.Write("Có lỗi xảy ra trong quá trình xử lý. Vui lòng thao tác lại!");
                    break;
            }
        %>
        </div>
    <script>
        function CheckForm() {
            //if (document.getElementById("MonHoc").checked == false) {
            //    alert("Chưa chọn môn học.");
            //    return false;
            //}
            if (document.getElementById("Ngay2").checked == true && document.getElementById("Ngay3").checked == true && document.getElementById("Ngay4").checked == true && document.getElementById("Ngay5").checked == true && document.getElementById("Ngay6").checked == true && document.getElementById("Ngay7").checked == true && document.getElementById("Ngay8").checked == true) {
                alert("Bạn chọn tất cả các ngày đều bận nên không sắp được thời khóa biểu.");
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#SapThoiKhoaBieu').addClass('active');
        });
    </script>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
