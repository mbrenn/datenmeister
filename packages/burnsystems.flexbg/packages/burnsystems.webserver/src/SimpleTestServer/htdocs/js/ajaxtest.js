
$(function()
{
	$("#Add").click(function()
	{
		var summand1 = $("#Summand1").val();
		var summand2 = $("#Summand2").val();

		$.ajax(
			'controller/Calculator/Sum',
			{
				data: 
				{
					Summand1: summand1,
					Summand2: summand2
				}, 
				type: 'POST'
			})
			.success(function(result)
			{
				$("#Sum").text(result.Sum);
			});
	});
});