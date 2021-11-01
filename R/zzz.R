imports "ggplot" from "MSImaging";

require(ggplot);
require(mzkit);

const .onLoad as function() {
    options(mzdiff = "da:0.3");
}