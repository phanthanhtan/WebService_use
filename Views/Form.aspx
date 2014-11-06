<div align="right">
<head>
    <style type="text/css">
        #submit1, #submit2, #submit3{
            width: 80px;
        }
    </style>
</head>
<%  //http://localhost:1302/TESTERS/Default6.aspx
    String path = HttpContext.Current.Request.Url.AbsolutePath;
    //path = "/TESTERS/Default6.aspx"
    String cotroller = "";
    for (int i = 1; i < path.Length; i++)
    {
        if (path[i].ToString() != "/")
            cotroller = cotroller + path[i];
        else
            break;
    }
%>
<form action="/<%=cotroller %>/Submit" method="post" name="Form" id="Form" enctype = "multipart/form-data" onsubmit = "return CheckForm()">
<table>
    <tr>
        <td>Mã sinh viên:</td>
        <td><input id="txtMaSinhVien" name="txtMaSinhVien" onkeypress="return CheckNumber(event)"/></td>
        <td><input type="submit" id="submit1" name="submit" value="Xem" /></td>
        <td><input type="submit" id="submit2" name="submit" value="Cập nhật" /></td>
        <td><input type="submit" id="submit3" name="submit" value="Xóa" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')" /></td>
    </tr>
</table>
</form>
<script>
    function CheckForm() {
        if (txtMaSinhVien.value == "" || txtMaSinhVien.value == null) {
            alert("Chưa nhập mã sinh viên!");
            txtMaSinhVien.focus();
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
</div>