-- Check if the table exists before attempting to delete data
if sql.TableExists("damage_data") then
    print("SQL Table Found: Deleting Old Data")
    -- Clear the table to prevent the file from getting too large
    sql.Query("DELETE FROM damage_data")
else
    print("SQL Table Not Found: Creating Table")
    -- Create the table since it doesn't exist
    sql.Query("CREATE TABLE damage_data (id INTEGER PRIMARY KEY AUTOINCREMENT, damage_type TEXT, hitbox TEXT, direction TEXT)")
end

-- Creat a table of damage types for the hook
DmgTypeMap = {
    [DMG_GENERIC] = "DMG_GENERIC",
    [DMG_CRUSH] = "DMG_CRUSH",
    [DMG_BULLET] = "DMG_BULLET",
    [DMG_SLASH] = "DMG_SLASH",
    [DMG_BURN] = "DMG_BURN",
    [DMG_VEHICLE] = "DMG_VEHICLE",
    [DMG_FALL] = "DMG_FALL",
    [DMG_BLAST] = "DMG_BLAST",
    [DMG_CLUB] = "DMG_CLUB",
    [DMG_SHOCK] = "DMG_SHOCK",
    [DMG_SONIC] = "DMG_SONIC",
    [DMG_ENERGYBEAM] = "DMG_ENERGYBEAM",
    [DMG_PREVENT_PHYSICS_FORCE] = "DMG_PREVENT_PHYSICS_FORCE",
    [DMG_NEVERGIB] = "DMG_NEVERGIB",
    [DMG_ALWAYSGIB] = "DMG_ALWAYSGIB",
    [DMG_DROWN] = "DMG_DROWN",
    [DMG_PARALYZE] = "DMG_PARALYZE",
    [DMG_NERVEGAS] = "DMG_NERVEGAS",
    [DMG_POISON] = "DMG_POISON",
    [DMG_RADIATION] = "DMG_RADIATION",
    [DMG_DROWNRECOVER] = "DMG_DROWNRECOVER",
    [DMG_ACID] = "DMG_ACID",
    [DMG_SLOWBURN] = "DMG_SLOWBURN",
    [DMG_REMOVENORAGDOLL] = "DMG_REMOVENORAGDOLL",
    [DMG_PHYSGUN] = "DMG_PHYSGUN",
    [DMG_PLASMA] = "DMG_PLASMA",
    [DMG_AIRBOAT] = "DMG_AIRBOAT",
    [DMG_DISSOLVE] = "DMG_DISSOLVE",
    [DMG_BLAST_SURFACE] = "DMG_BLAST_SURFACE",
    [DMG_DIRECT] = "DMG_DIRECT",
    [DMG_BUCKSHOT] = "DMG_BUCKSHOT",
    [DMG_SNIPER] = "DMG_SNIPER",
    [DMG_MISSILEDEFENSE] = "DMG_MISSILEDEFENSE"
}

-- Hook into the EntityTakeDamage event
hook.Add("EntityTakeDamage", "OWOIntegration_DamageHook", function(ent, dmginfo)
    -- Check if the entity is the local player
    if ent == LocalPlayer() then
        print("OWOIntegration_DamageHook called")
        local dmgType = DmgTypeMap[dmginfo:GetDamageType()] or ""
        print("Damage type: " .. tostring(dmgType))

        -- Get the hitbox that was hit
        local hitbox = LocalPlayer():LastHitGroup()
        print("Hitbox: " .. tostring(hitbox))

        -- Calculate the relative direction of the damage origin
        local damageOrigin = dmginfo:GetDamageOrigin()
        local playerPos = LocalPlayer():GetPos()
        local direction = (damageOrigin - playerPos):GetNormalized()

        -- Simplify the direction to front, back, left, right, etc.
        local playerForward = LocalPlayer():GetAimVector()
        local dotProduct = direction:Dot(playerForward)

        if dotProduct > 0.5 then
            -- Front
            direction = "Front"
        elseif dotProduct < -0.5 then
            -- Back
            direction = "Back"
        else
            -- Side
            direction = (direction:Dot(LocalPlayer():GetRight()) > 0) and "Right" or "Left"
        end
        print("Direction: " .. tostring(direction))

        -- Insert the data into the database
        local query = string.format(
            "INSERT INTO damage_data (damage_type, hitbox, direction) VALUES (%s, %d, %s)",
            sql.SQLStr(dmgType),
            sql.SQLStr(hitbox),
            sql.SQLStr(direction)
        )
        
        local result = sql.Query(query)

        if result == false then
            print("SQL Error: " .. sql.LastError())
        else
            print("Data inserted into database successfully.")
        end
    end
end)