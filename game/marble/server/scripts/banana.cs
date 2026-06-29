//-----------------------------------------------------------------------------
// Super Monkey Ball Banana Collectable Item
//-----------------------------------------------------------------------------

datablock AudioProfile(BananaPickupSound)
{
   filename    = "marble/data/sound/custom/banana_pickup.wav";
   description = AudioClosest3d;
   preload = true;
};

datablock ItemData(BananaItem)
{
   // Mission editor category
   category = "SuperMonkeyBall";
   className = "Banana";

   // Basic Item properties
   shapeFile = "marble/data/shapes/items/gem.dts"; // Using gem temporarily until banana shape is created
   mass = 1;
   friction = 1;
   elasticity = 0.3;

   // Dynamic properties defined by the scripts
   pickupName = "a Banana!";
   maxInventory = 1;

   pickupAudio = BananaPickupSound;
};

function BananaItem::onPickup(%this, %obj, %user, %amount)
{
   // Check if we have a valid client
   if (%user.client)
   {
      // Play pickup sound
      serverPlay3D(%this.pickupAudio, %obj.getTransform());

      // Inform client
      messageClient(%user.client, 'MsgItemPickup', '\c0You picked up %1', %this.pickupName);

      // Increment bananas
      %val = ($Game::Collectables::BananaValue !$= "") ? $Game::Collectables::BananaValue : 1;
      %score = ($Game::Collectables::BananaScore !$= "") ? $Game::Collectables::BananaScore : 10;
      %threshold = ($Game::Collectables::ExtraLifeThreshold !$= "") ? $Game::Collectables::ExtraLifeThreshold : 100;

      %user.client.bananas += %val;
      %user.client.score += %score;

      // Check for extra life threshold
      if (%user.client.bananas >= %threshold)
      {
         %user.client.bananas -= %threshold;
         %user.client.lives++;
         messageClient(%user.client, 'MsgExtraLife', '\c0Extra Life!');
         bottomPrint(%user.client, "EXTRA LIFE!", 2000, 3);
         // Play extra life sound here
      }

      // Update client UI
      commandToClient(%user.client, 'SetBananaCount', %user.client.bananas);
   }

   // Parent method handles respawn/deletion
   Parent::onPickup(%this, %obj, %user, %amount);
   return true;
}
