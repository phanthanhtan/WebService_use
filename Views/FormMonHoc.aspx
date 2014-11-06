<div align="right">
<head>
    <style type="text/css">
        #submit1, #submit2, #submit3{
            width: 50px;
        }
    </style>
</head>
        <table>
            <tr>
                <%--<td><form action="/SXTKB/SXTKB_MonHoc/Submit" method="post" name="Form" id="Form" enctype="multipart/form-data" onsubmit="return CheckForm()"></td>
                <td>Mã môn học:</td>
                <td>
                    <input id="txtMaMonHoc" name="txtMaMonHoc" /></td>
                <td>
                    <input type="submit" id="submit1" name="submit1" value="Xem" /></td>
                <td>
                    <input type="submit" id="submit2" name="submit1" value="Sửa" /></td>
                <td>
                    <input type="submit" id="submit3" name="submit1" value="Xóa" onclick="return confirm('Bạn chắc chắn muốn xóa?!?')" /></td>
                <td></form></td>
                <td><a href="/SXTKB_MonHoc/ThemMonHoc" style="color: black"><button>Thêm môn học</button></a></td>--%>
                <td><a href="/SXTKB_MonHoc/ThemMonHoc">Thêm môn học</a></td>
            </tr>
        </table>
    
    <script>
        function CheckForm() {
            if (txtMaMonHoc.value == "" || txtMaMonHoc.value == null) {
                alert("Chưa nhập mã môn học!");
                txtMaMonHoc.focus();
                return false;
            }
        }
    </script>
</div>