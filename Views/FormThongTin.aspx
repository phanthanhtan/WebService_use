<%  WebService_use.Models.ThongTin tt = (WebService_use.Models.ThongTin)ViewBag.ThongTin;
    %>
    <table>
        <tr><td class="text-align-right">Mã sinh viên:</td><td><%=tt.MaSinhVien %></td></tr>
        <tr><td class="text-align-right">Tên sinh viên:</td><td><%=tt.TenSinhVien %></td></tr>
        <tr><td class="text-align-right">Lớp:</td><td><%=tt.Lop %></td></tr>
        <tr><td class="text-align-right">Ngành:</td><td><%=tt.Nganh.TenNganh %></td></tr>
        <tr><td class="text-align-right">Khoa:</td><td><%=tt.Khoa.TenKhoa %></td></tr>
        <tr><td class="text-align-right">Hệ đào tạo:</td><td><%=tt.HeDaoTao.TenHeDaoTao %></td></tr>
        <tr><td class="text-align-right">Khóa học:</td><td><%=tt.KhoaHoc.TenKhoaHoc %></td></tr>
        <tr><td></td><td><a href="/ThongTin/CapNhat/<%=tt.MaSinhVien %>">Cập nhật thông tin cá nhân</a></td></tr>
    </table>
    <br />
    <%
%>