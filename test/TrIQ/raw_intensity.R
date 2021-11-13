require(mzkit);
require(ggplot);

imports "MsImaging" from "mzplot";

options(strict = FALSE);
options(memory.load = "max");

data = "E:\demo\CleanSample.mzPack"
|> open.mzpack
|> viewer
|> MSIlayer(mz = 153.14)
|> intensity
;

print(data);

data = data.frame(intensity = data);

bitmap(file = `${@dir}/intensity_hist.png`, size = [1800, 1400]) {
	ggplot(data, aes(x = "intensity"), padding = "padding:200px 400px 200px 200px;")
	  + geom_histogram(bins = 100,  color = "steelblue")
	  + ggtitle("Frequency of intensity")
	  + scale_x_continuous(labels = "G2")
      + scale_y_continuous(labels = "F0")
	  + theme_default()
	  + theme(plot.title = element_text(size = 16), axis.text=element_text(size=8))
	  ;
}

bitmap(file = `${@dir}/logIntensity_hist.png`, size = [2000, 1400]) {
	ggplot(data.frame(log_intensity = log(data[, "intensity"])), aes(x = "log_intensity"), padding = "padding:200px 500px 200px 200px;")
	  + geom_histogram(bins = 100,  color = "steelblue")
	  + ggtitle("Frequency of intensity")
	  + scale_x_continuous(labels = "G2")
      + scale_y_continuous(labels = "F0")
	  + theme_default()
	  + theme(plot.title = element_text(size = 16), axis.text=element_text(size=8))
	  ;
}

data = data[, "intensity"];

qcut = MsImaging::TrIQ(data, q = 0.6);
qcut = max(data) * max(qcut);

print(qcut);

data[data > qcut] = qcut;

bitmap(file = `${@dir}/TrIQ_intensity_hist.png`, size = [2400, 1400]) {
	ggplot(data.frame(TrIQ_intensity = data), aes(x = "TrIQ_intensity"), padding = "padding:200px 500px 200px 200px;")
	  + geom_histogram(bins = 100,  color = "steelblue")
	  + ggtitle("Frequency of intensity")
	  + scale_x_continuous(labels = "G2")
      + scale_y_continuous(labels = "F0")
	  + theme_default()
	  + theme(plot.title = element_text(size = 16), axis.text=element_text(size=8))
	  ;
}

