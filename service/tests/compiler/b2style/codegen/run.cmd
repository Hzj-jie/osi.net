
setlocal
path %PATH%;..\..\..\..\resource\gen;
gen _b2style_test_data case1 case1.txt case2 case2.txt bool_and_bool bool_and_bool.txt str_unescape str_unescape.txt _1_to_100 _1_to_100.txt self_add self_add.txt biguint biguint.txt negative_int negative_int.txt another_1_to_100 another_1_to_100.txt loaded_method loaded_method.txt ufloat_std_out ufloat_std_out.txt ufloat_operators ufloat_operators.txt while_1_to_100 while_1_to_100.txt while_0_to_1 while_0_to_1.txt pi_integral_0_1 pi_integral_0_1.txt calculate_pi_bbp calculate_pi_bbp.txt shift shift.txt order_of_operators order_of_operators.txt include include.txt include2 include2.txt ifndef ifndef.txt > b2style_test_data.vb
move /Y b2style_test_data.vb ..\
endlocal

