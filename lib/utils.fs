: 3DUP  ( a b c -- a b c a b c )  DUP 2OVER ROT ;

: wide_type ( cell_addr len -- )
    0 ?do
        dup i cells + c@ emit
    loop
;

: wide_str>s ( cell_addr len -- s )
    0 swap
    0 ?do
        10 *
        over i cells + c@ parse_if_digit if + else leave then
    loop
    nip
;

: to_digit ( char - num )
    48 -
;

: is_digit ( char - flag )
    [ 0 48 + ] LITERAL [ 10 48 + ] LITERAL within 
    0<>
;

: parse_if_digit ( num - [num] flag )
    dup
    is_digit if to_digit true else drop false then  
;
