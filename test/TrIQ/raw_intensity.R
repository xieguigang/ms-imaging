require(mzkit);
require(ggplot);

imports "MsImaging" from "mzplot";

options(strict = FALSE);
options(memory.load = "max");

data = "E:\\FULL MS_centriod_CHCA_20210819.mzPack"
|> open.mzpack
|> viewer
|> MSIlayer(mz = 772.5206)
|> intensity
;

print(data);

hist_data = as.data.frame(hist(data, step = 5000));
hist_data = hist_data[order(hist_data[, "bin_size"], decreasing = TRUE), ];
data = data.frame(intensity = data);

print("view of the histogram data:");
print(hist_data, max.print = 6);

print("evaluate the gini index:");
print(gini(hist_data[, "bin_size"]));

bitmap(file = `${@dir}/intensity_hist.png`, size = [1800, 1400]) {
	ggplot(data, aes(x = "intensity"), padding = "padding:200px 400px 200px 200px;")
	  + geom_histogram(bins = 100,  color = "steelblue")
	  + ggtitle(`Frequency of intensity(gini: ${round(gini(hist_data[, "bin_size"]), 2)})`)
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

# apply of the TrIQ cutoff
data[data > qcut] = qcut;
hist = as.data.frame(hist(data, step = 5000));
hist = hist[order(hist[, "bin_size"], decreasing = TRUE), ];

print("evaluate the new gini index:");
print(gini(hist[, "bin_size"]));

bitmap(file = `${@dir}/TrIQ_intensity_hist.png`, size = [2400, 1400]) {
	ggplot(data.frame(TrIQ_intensity = data), aes(x = "TrIQ_intensity"), padding = "padding:200px 500px 200px 200px;")
	  + geom_histogram(bins = 100,  color = "steelblue")
	  + ggtitle(`Frequency of TRIQ intensity(gini: ${round(gini(hist[, "bin_size"]), 2)})`)
	  + scale_x_continuous(labels = "G2")
      + scale_y_continuous(labels = "F0")
	  + theme_default()
	  + theme(plot.title = element_text(size = 16), axis.text=element_text(size=8))
	  ;
}

