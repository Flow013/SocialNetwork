CREATE INDEX IF NOT EXISTS users_first_name_idx 
	ON public.user(first_name text_pattern_ops);

CREATE INDEX IF NOT EXISTS users_second_name_idx 
	ON public.user(second_name text_pattern_ops);
