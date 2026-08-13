using System.Numerics;
using Assimp;
using Assimp.Configs;
using Raylib_cs;
using AssimpMesh = Assimp.Mesh;
using AssimpScene = Assimp.Scene;
using Mesh = Raylib_cs.Mesh;

namespace Engine.Graphics
{
    public static class MeshLoader
    {
        public static List<Mesh> CreateFromAssimp(string file)
        {
            List<Mesh> meshes = [];
            AssimpContext importer = new();

            try
            {
                importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
                AssimpScene scene = importer.ImportFile(file, PostProcessPreset.TargetRealTimeMaximumQuality);

                foreach (AssimpMesh sceneMesh in scene.Meshes)
                {
                    Mesh mesh = new()
                    {
                        VertexCount = sceneMesh.VertexCount,
                        TriangleCount = sceneMesh.FaceCount
                    };

                    mesh.AllocVertices();
                    Span<Vector3> vertices = mesh.VerticesAs<Vector3>();
                    for (int i = 0; i < sceneMesh.VertexCount; i++)
                    {
                        vertices[i] = new Vector3(
                            sceneMesh.Vertices[i].X, sceneMesh.Vertices[i].Y, sceneMesh.Vertices[i].Z
                        );
                    }

                    if (sceneMesh.HasNormals)
                    {
                        mesh.AllocNormals();

                        Span<Vector3> normals = mesh.NormalsAs<Vector3>();
                        for (int i = 0; i < sceneMesh.VertexCount; i++)
                        {
                            normals[i] = new Vector3(
                                sceneMesh.Normals[i].X, sceneMesh.Normals[i].Y, sceneMesh.Normals[i].Z
                            );
                        }
                    }

                    if (sceneMesh.HasTangentBasis)
                    {
                        mesh.AllocTangents();

                        Span<Vector3> tangents = mesh.TangentsAs<Vector3>();
                        for (int i = 0; i < sceneMesh.VertexCount; i++)
                        {
                            tangents[i] = new Vector3(
                                sceneMesh.Tangents[i].X, sceneMesh.Tangents[i].Y, sceneMesh.Tangents[i].Z
                            );
                        }
                    }

                    if (sceneMesh.HasTextureCoords(0))
                    {
                        mesh.AllocTexCoords();

                        Span<Vector2> texCoords = mesh.TexCoordsAs<Vector2>();
                        for (int i = 0; i < sceneMesh.VertexCount; i++)
                        {
                            texCoords[i] = new Vector2(
                                sceneMesh.TextureCoordinateChannels[0][i].X,
                                sceneMesh.TextureCoordinateChannels[0][i].Y
                            );
                        }
                    }

                    mesh.AllocIndices();

                    List<ushort> indicesList = [];
                    for (int i = 0; i < sceneMesh.FaceCount; i++)
                    {
                        Face face = sceneMesh.Faces[i];
                        indicesList.Add((ushort)face.Indices[1]);
                        indicesList.Add((ushort)face.Indices[2]);
                        indicesList.Add((ushort)face.Indices[0]);

                        if (face.IndexCount != 4)
                        {
                            continue;
                        }

                        indicesList.Add((ushort)face.Indices[2]);
                        indicesList.Add((ushort)face.Indices[3]);
                        indicesList.Add((ushort)face.Indices[0]);
                    }

                    Span<ushort> indices = mesh.IndicesAs<ushort>();
                    for (int i = 0; i < indices.Length; i++)
                    {
                        indices[i] = indicesList[i];
                    }

                    Raylib.UploadMesh(ref mesh, false);
                    meshes.Add(mesh);
                }

                return meshes;
            }
            finally
            {
                importer.Dispose();
            }
        }
    }
}