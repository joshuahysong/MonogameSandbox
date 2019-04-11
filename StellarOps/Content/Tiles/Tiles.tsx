<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.2" tiledversion="1.2.3" name="Tiles" tilewidth="64" tileheight="64" tilecount="25" columns="5">
 <image source="Tiles.png" trans="ff00ff" width="320" height="320"/>
 <tile id="0" type="Hull">
  <properties>
   <property name="Collision" type="int" value="1"/>
   <property name="Health" type="int" value="100"/>
   <property name="Name" value="Hull"/>
   <property name="Type" value="Hull"/>
  </properties>
 </tile>
 <tile id="1" type="Floor">
  <properties>
   <property name="Collision" type="int" value="2"/>
   <property name="Health" type="int" value="100"/>
   <property name="Name" value="Floor"/>
   <property name="Type" value="Floor"/>
  </properties>
 </tile>
 <tile id="2" type="Floor">
  <properties>
   <property name="Collision" type="int" value="2"/>
   <property name="Health" type="int" value="100"/>
   <property name="Name" value="Flight Control"/>
   <property name="Type" value="FlightControl"/>
  </properties>
 </tile>
 <tile id="5">
  <properties>
   <property name="Collision" value="1"/>
   <property name="Health" value="200"/>
   <property name="Name" value="Maneuvering Thruster"/>
   <property name="Type" value="Thruster"/>
  </properties>
 </tile>
 <tile id="6">
  <properties>
   <property name="Type" value="Subcomponent"/>
  </properties>
 </tile>
 <tile id="7" type="Subcomponent">
  <properties>
   <property name="Type" value="Subcomponent"/>
  </properties>
 </tile>
 <tile id="8" type="Subcomponent">
  <properties>
   <property name="Type" value="Subcomponent"/>
  </properties>
 </tile>
 <tile id="9" type="Subcomponent">
  <properties>
   <property name="Type" value="Subcomponent"/>
  </properties>
 </tile>
 <tile id="10">
  <properties>
   <property name="Type" value="Effect"/>
  </properties>
 </tile>
 <tile id="11">
  <properties>
   <property name="Type" value="Effect"/>
  </properties>
 </tile>
 <tile id="12">
  <properties>
   <property name="Type" value="Effect"/>
  </properties>
 </tile>
 <tile id="13">
  <properties>
   <property name="Type" value="Effect"/>
  </properties>
 </tile>
 <tile id="14">
  <properties>
   <property name="Type" value="Effect"/>
  </properties>
 </tile>
</tileset>
