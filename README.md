

# Cut and Fill

Measure cut and fill volumes for a site.

|Input Name|Type|Description|
|---|---|---|
|Site Perimeter|https://hypar.io/Schemas/Geometry/Polygon.json|The perimeter of the site.|
|Perimeters|array|The perimeters of the cut and fill areas.|
|Elevation|number|The elevation of the final site relative to the site origin.|
|Batter Angle|number|The angle of fill battering in degrees.|


<br>

|Output Name|Type|Description|
|---|---|---|
|Cut|Number|The cut volume.|
|Fill|Number|The fill volume.|
|Excavation Cost|String|The cost to excavate soil on the site.|
|Site Balancing Cost|String|The cost to balance cut and fill on the site.|

