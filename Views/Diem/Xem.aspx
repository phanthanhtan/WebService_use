<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Layout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--#include file="../Form.aspx"-->
    <div>
        <div align="center">
            <h2>Lịch thi cá nhân</h2>
            <!--#include file="../FormThongTin.aspx"-->
        </div>
            <%  int dem = 0;
                foreach (WebService_use.Models.Diem_ChiTiet_Xem diem_ct in (List<WebService_use.Models.Diem_ChiTiet_Xem>)ViewBag.Diem_ChiTiet_Xem)
                {
                    if (dem == 0)
                    {%>
                        <div style="margin-bottom:10px;">
                            <b>Tất cả học kỳ:</b>
                            <a href="/Diem/CapNhat/<%=diem_ct.MaSinhVien %>">Cập nhật</a> - 
                            <a href="/Diem/Xoa/<%=diem_ct.MaSinhVien %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa</a>
                            <br />
                            <form action="/Diem/CapNhatForm/<%=diem_ct.MaSinhVien %>" method="post" name="FDiemMaHocKy" id="FDiemMaHocKy" enctype = "multipart/form-data" onsubmit = "return Check()" >
                                Mã học kỳ: <input type="text" id="txtMaHocKy" name="txtMaHocKy" /> <input type="submit" id="submitMaMonHoc" name="submitMaMonHoc" value="Cập nhật" />
                            </form>
                            <script>
                                function Check() {
                                    if (txtMaHocKy.value == "" || txtMaHocKy.value == null) {
                                        alert("Chưa nhập mã học kỳ.");
                                        txtMaHocKy.focus();
                                        return false;
                                    }
                                }
                            </script>
                        </div>
                        <table style="margin-left:-4px"><tr><td>
                        <select id="_MaHocKy" onchange="return CheckHocKy('/Diem/Xem/<%=diem_ct.MaSinhVien %>/');">
                    <%  dem = 1;
                    }%>
                            <option id="<%=diem_ct.MaHocKy %>" value="<%=diem_ct.MaHocKy %>" <%if (ViewBag.MaHocKy == diem_ct.MaHocKy) {%>selected<%} %>><%=diem_ct.TenHocKy %></option>
                <%}%>
                        </select>
                        </td><td><a href="/Diem/CapNhat/<%=ViewBag.mssv %>/<%=ViewBag.MaHocKy %>">Cập nhật</a> - 
                        <a href="/Diem/Xoa/<%=ViewBag.mssv %>/<%=ViewBag.MaHocKy %>" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')">Xóa</a></td>
                        </tr>
                        </table>
                <%  int dem1 = 0, stt = 1;
                    foreach (WebService_use.Models.Diem_ChiTiet diem_ct in (List<WebService_use.Models.Diem_ChiTiet>)ViewBag.Diem_ChiTiet)
                    {
                        if (dem1 == 0)
                        {%>
                            <table cellspacing="0" cellpadding="0" border="1">
                                <tr style="text-align: center">
                                    <td>STT</td>
                                    <td>Mã môn học</td>
                                    <td>Tên môn học</td>
                                    <td>TC</td>
                                    <td>% KT</td>
                                    <td>% Thi</td>
                                    <td>Điểm KT</td>
                                    <td>Điểm thi</td>
                                    <td>TK (10)</td>
                                    <td>TK (chữ)</td>
                                </tr>
                        <%dem1 = 1;
                        }%>
                                <tr style="text-align: center">
                                    <td><%=stt%></td>
                                    <td><%=diem_ct.MaMonHoc%></td>
                                    <td style="text-align: left"><%=diem_ct.MonHoc.TenMonHoc%></td>
                                    <td><%=diem_ct.SoTinChi%></td>
                                    <td><%=diem_ct.PhanTramKiemTra%></td>
                                    <td><%=diem_ct.PhanTramThi%></td>
                                    <td><%=diem_ct.DiemKiemTra%></td>
                                    <td><%=diem_ct.DiemThiLanMot%></td>
                                    <td><%=diem_ct.DiemTongKetHeMuoi%></td>
                                    <td><%=diem_ct.DiemTongKetDiemChu%></td>
                                </tr>
                    <%stt++;
                    }
                    if (ViewBag.Diem_TongQuat != null)//không phải học kỳ 3 => có điểm tổng quát
                    {
                        WebService_use.Models.Diem_TongQuat diem_tq = (WebService_use.Models.Diem_TongQuat)ViewBag.Diem_TongQuat;
                        %>
                            <tr>
                                <td colspan="3">Điểm trung bình học kỳ hệ 10:</td><td colspan="7"><%=diem_tq.DiemTrungBinhHocKyHeMuoi %></td>
                            </tr>
                            <tr>
                                <td colspan="3">Điểm trung bình học kỳ hệ 4:</td><td colspan="7"><%=diem_tq.DiemTrungBinhHocKyHeBon %></td>
                            </tr>
                            <tr>
                                <td colspan="3">Điểm trung bình tích lũy hệ 10:</td><td colspan="7"><%=diem_tq.DiemTrungBinhTichLuyHeMuoi %></td>
                            </tr>
                            <tr>
                                <td colspan="3">Điểm trung bình tích lũy hệ 4:</td><td colspan="7"><%=diem_tq.DiemTrungBinhTichLuyHeBon %></td>
                            </tr>
                            <tr>
                                <td colspan="3">Số tín chỉ đạt:</td><td colspan="7"><%=diem_tq.SoTinChiDat %></td>
                            </tr>
                            <tr>
                                <td colspan="3">Số tín chỉ tích lũy:</td><td colspan="7"><%=diem_tq.SoTinChiTichLuy %></td>
                            </tr>
                            <tr>
                                <td colspan="3">Điểm trung bình rèn luyện học kỳ:</td><td colspan="7"><%=diem_tq.DiemTrungBinhRenLuyenHocKy %></td>
                            </tr>
                            <tr>
                                <td colspan="3">Phân Loại ĐTBRL HK:</td><td colspan="7"><%=diem_tq.PhanLoaiDTBRenLuyenHocKy %></td>
                            </tr>
                    <%}%>
                    </table>
                <%
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
            $('#Diem').addClass('active');
        });
    </script>
    <script>
        function CheckHocKy(link) {
            //alert(link + _MaTuan.value.toString());
            window.location = link + _MaHocKy.value.toString();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
