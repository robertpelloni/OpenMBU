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
      %user.client.bananas++;

      // Every 100 bananas is an extra life (SMB standard)
      if (%user.client.bananas >= 100)
      {
         %user.client.bananas -= 100;
         %user.client.lives++;
         messageClient(%user.client, 'MsgExtraLife', '\c0Extra Life!');
         // Play extra life sound here
      }

      // Update client UI
      commandToClient(%user.client, 'SetBananaCount', %user.client.bananas);
   }

   // Parent method handles respawn/deletion
   Parent::onPickup(%this, %obj, %user, %amount);
   return true;
}
