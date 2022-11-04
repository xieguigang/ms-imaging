imports "ggplot" from "MSImaging";
imports "MSIfilter" from "MSImaging";

require(ggplot);
require(mzkit);

const .onLoad = function() {
    options(mzdiff = "da:0.3");
}