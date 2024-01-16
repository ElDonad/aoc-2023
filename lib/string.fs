: str_in ( str max_usize substr usize -- flag )
    { str max_usize substr usize }
    max_usize usize < if false exit then

    str usize substr usize compare 0= if true else false then
;

: str_strip ( str usize -- str2 usize2 ) 
    dup 0 ?do
        over i + c@ BL <> if i - swap i + swap unloop exit then
    loop 

;

: str_print_array ( str_array size  -- )
    0 ?do
        type cr
    loop
;

: str>s ( str usize -- s )
    0 s>d 2swap
    >NUMBER 2drop
    d>s
;

: str_split ( str usize separator_str usize2 -- [occurence_addr usize]*n n )
    { str usize separator_str usize2 }
    1
    str \ last pos
    usize usize2 - 0 ?do
        str i + 
        usize i - 
        separator_str usize2
        str_in if
            dup str i + swap - \ set size of previous string
            rot 1 + 
            str i + usize2 + \ new position
            usize2 1 +
        else
            1
        then
    +loop
    dup str usize + swap - \ set size of last string
    rot
;
