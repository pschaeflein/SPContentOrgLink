var MaventionExecuteSodFunction = function (fn, scriptKey) {
	if (!SP.SOD.executeOrDelayUntilScriptLoaded(fn, scriptKey)) {
		LoadSodByKey(NormalizeSodKey(scriptKey));
	}
}