param ([string]$proj= 'E:\Simon\TFSOnline\ExposureBasedModelling\ReIns\Code\EBM\EBM_AggregateCube\EBM_AggregateCube.dwproj')

$vsPath = 'C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\devenv.com'
$configuration = "Development"

& $vspath $proj /build $configuration /project $proj /projectconfig $configuration