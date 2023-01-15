require(MSImaging);

# autoSize = function(dims, padding, 
                          # scale = 1, 
                          # is_multiple_combine_wide = FALSE, 
                          # ratio_threshold = 1.5) {

str(MSImaging::autoSize({w: 79, h: 367}, scale = 13, padding = "padding: 200px 600px 200px 250px;", ratio_threshold = 1.2));