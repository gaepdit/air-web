# Data migration steps

1. Run the application to create the new database tables, then shut it down. (This can be done locally, but make sure
   the application is not running on the server.)

2. Run the scripts in the `1-setup` folder to create temporary DB objects used by the migration scripts.

3. Run the data cleanup scripts in the `2-data-cleanup` folder.

4. Run the migration scripts in the `3-data-migration` folder.

5. When ready, run the cleanup scripts in the `4-cleanup` folder to remove the temporary objects.
