require(MSImaging);

# autoSize = function(dims, padding, 
                          # scale = 1, 
                          # is_multiple_combine_wide = FALSE, 
                          # ratio_threshold = 1.5) {

str(MSImaging::autoSize({w: 39, h: 339}, scale = 8, padding = "padding: 200px 600px 200px 250px;", ratio_threshold = 1.2));