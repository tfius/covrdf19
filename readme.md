# LDA Estimator Model Trainer and document search over COVID-19 Open Research Dataset

## This solution contains 
 - covrdf (file preparation, training and building results) 
 - covrd (web server with API endpoints and search in documents) 
 

## Buiilding your own covrd 
Download CORD-19 dataset and unpack all its parts into somefolder

open appsettings.json in covrdf project
 1.specifiy location where cord-19 dataset is located 
    "root": "C:\\Users\\user\\Downloads\\covid-19 cord-19\\",
 
 2.specify input csv   
    "metadata": "all_sources_metadata_2020-03-13.csv",

 3.specify folders with files to be combined 
	"folders": [ "biorxiv", "czi", "pmc", "pmc-comm" ],   
	
 4.Run "covrdf" 
    wait for project to finish
	
 5. check file exists in project "covrd\Resources\combined_papers.json" 
 
 6. check file exists in project "covrd\ML\lda_predictionengine_all.zip" 
  
 You can now build and run **covrd** project and search data  
 

## 

The COVRD web app, trainer and tools are free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

The COVRD web app, trainer and tools are distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with the FairDataSociety library. If not, see <http://www.gnu.org/licenses/>.

##
Dedicated to a wonderful human being, best grandmother and my mom. 
May the stars shine bright in her afterlive.  

Written by Tadej Fius in 2020.
 

