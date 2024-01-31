$(document).ready(function () {
    $('#myTable').DataTable({
        "order": [],
        "lengthMenu": [[100, 200, 300, -1], ["100", "200", "300", "All"]],
        "pageLength": 100
    });
}); 