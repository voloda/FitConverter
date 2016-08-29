# Sigma .smf convertor to .fit file

This utility allows to convert Sigma .smf file exported from Sigma Data Center to .fit
file.

Utility is trying to reconstruct the training so the result is near to the original .smf
file.

Since some data are missing the produced output is not the exact one.

## Command line:
```
> FitConverter.exe 2016\*.smf
```

or

```
> FitConverter.exe 2016_06_28__18_22_KolemPrahy.fit
```

## Comments for Strava:

* Training time can slightly vary since activity takes into account zone times only
* Elevation can differ since Strava is trying to calculate it
* Incline/decline is not converted
* Activity time must be populated

## Support

If you need to get a support populate an issue. Please attach example .smf file
to your request.

## License

* This utility is using FitSDK (Fit.dll)
