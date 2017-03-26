$(document).ready(function () {
    $(function () {
    $('#minPrice').change(function () {       
            var selectedMaxValue = Number($('#maxPrice').val());
            var selectedMinValue = Number($('#minPrice').val());
        if (selectedMaxValue > 0 && selectedMinValue > selectedMaxValue) {                 
                alert('Maximum Price cannot be less than Minimum Price');                          
            $("#minPrice").val("0").change();          
            }          
        });
    });
    $(function () {
        $('#maxPrice').change(function () {
            var selectedMinValue = Number($('#minPrice').val());
            var selectedMaxValue = Number($('#maxPrice').val());
            if (selectedMaxValue > 0 && selectedMinValue > selectedMaxValue) {
                alert('Minimum Price cannot be More than Maximum Price');
                $("#maxPrice").val("0").change();
            }
        });
    });    
});